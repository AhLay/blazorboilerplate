using BlazorBoilerplate.Application;
using BlazorBoilerplate.Application.Implementations;
using BlazorBoilerplate.Shared.Dto;
using BlazorBoilerplate.Shared.Dto.Account;
using MatBlazor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorBoilerplate.Components.Mat.Admins.Views
{
    [Authorize]
    public partial class UsersComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }

        [Inject]
        protected IAuthorizationService AuthorizationService { get; set; }

        [Inject]
        protected IIdentityAuthStateService AuthStateService { get; set; }

        [Inject]
        protected IMatToaster MatToaster { get; set; }

        [Inject]
        protected AppState AppStateService { get; set; }

        private int pageSize { get; set; } = 15;
        private int currentPage { get; set; } = 0;

        protected bool CreateUserDialogOpen { get; set; } = false;

        protected List<UserInfoDto> Users { get; set; }

        protected RegisterDto RegistrationParameters { get; set; } = new RegisterDto();

        protected UserProfileDto UserProfile { get; set; } = new UserProfileDto();

        protected bool IsEditDialogOpen { get; set; } = false;

        protected List<RoleItem> SelectedRoleItems { get; set; } = new List<RoleItem>();

        protected bool IsDeleteUserDialogOpen { get; set; } = false;
        protected bool IsResetPasswordDialogOpen { get; set; } = false;

        //Auth documentation
        //https://docs.microsoft.com/en-us/aspnet/core/security/blazor
        public UserInfoDto user { get; set; } = new UserInfoDto(); // Holds user being actively modified or created

        protected override async Task OnInitializedAsync()
        {
            await RetrieveUserListAsync();
            await PopulateRoleList();
            UserProfile = await AppStateService.GetUserProfile();
        }

        public async Task RetrieveUserListAsync()
        {
            try
            {
                var apiResponse = await Http.GetFromJsonAsync<ApiResponseDto<List<UserInfoDto>>>($"api/Admin/Users?pageSize={pageSize}&pageNumber={currentPage}");

                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add(apiResponse.Message, MatToastType.Success, "Users Retrieved");
                    Users = apiResponse.Result;
                }
                else
                    MatToaster.Add(apiResponse.Message + " : " + apiResponse.StatusCode, MatToastType.Danger, "User Retrieval Failed");
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Retrieval Error");
            }
        }

        public void OpenEditDialog(Guid userId)
        {
            user = new UserInfoDto(); // reset user object
            user = Users.Where(x => x.UserId == userId).SingleOrDefault();  // load the user information into the temp user object for worry free editing
            foreach (var role in SelectedRoleItems)
                role.IsSelected = user.Roles.Contains(role.Name);

            user.SaveState();
            IsEditDialogOpen = true;
        }

        public void OpenResetPasswordDialog(string userName, Guid userId)
        {
            // reusing the registerParameters and User objects
            RegistrationParameters = new RegisterDto();  // reset object
            RegistrationParameters.UserName = userName;
            user.UserId = userId;
            IsResetPasswordDialogOpen = true;
        }

        public void OpenDeleteDialog(Guid userId)
        {
            user = Users.Where(x => x.UserId == userId).SingleOrDefault();
            IsDeleteUserDialogOpen = true;
        }

        protected void UpdateUserRole(RoleItem roleSelectionItem)
        {
            roleSelectionItem.IsSelected = !roleSelectionItem.IsSelected;
        }

        public async Task PopulateRoleList()
        {
            try
            {
                var roleNames = new List<string>();
                var response = await Http.GetFromJsonAsync<ApiResponseDto<List<string>>>("api/Account/ListRoles");

                roleNames = response.Result;

                SelectedRoleItems = new List<RoleItem>();// clear out list

                // initialize selection list with all un-selected
                foreach (string role in roleNames)
                {
                    SelectedRoleItems.Add(new RoleItem
                    {
                        Name = role,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Role Retrieval Error");
            }
        }

        public void CancelChanges()
        {
            user.RestoreState();
            IsEditDialogOpen = false;
        }

        public async Task UpdateUserAsync()
        {
            try
            {
                //update the user object's role list with the new selection set
                user.Roles = SelectedRoleItems.Where(x => x.IsSelected == true).Select(x => x.Name).ToList();

                var apiResponse = await Http.PutJsonAsync<ApiResponseDto>("api/Account", user);

                if (apiResponse.IsSuccessStatusCode)
                    MatToaster.Add("User Updated", MatToastType.Success);
                else
                {
                    MatToaster.Add("Error", MatToastType.Danger, apiResponse.StatusCode.ToString());
                    user.RestoreState();
                }

                IsEditDialogOpen = false;
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Update Error");
            }
            finally
            {
                user.ClearState();
            }
        }

        public async Task CreateUserAsync()
        {
            try
            {
                if (RegistrationParameters.Password != RegistrationParameters.PasswordConfirm)
                {
                    MatToaster.Add("Password Confirmation Failed", MatToastType.Danger, "");
                    return;
                }

                var apiResponse = await AuthStateService.Create(RegistrationParameters);
                if (apiResponse.IsSuccessStatusCode)
                {
                    MatToaster.Add(apiResponse.Message, MatToastType.Success);
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoDto>(apiResponse.Result.ToString());
                    Users.Add(user);
                    RegistrationParameters = new RegisterDto(); //reset create user object after insert
                    CreateUserDialogOpen = false;
                }
                else
                {
                    MatToaster.Add(apiResponse.Message + " : " + apiResponse.StatusCode, MatToastType.Danger, "User Creation Failed");
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Creation Error");
            }
        }

        public async Task ResetUserPasswordAsync()
        {
            try
            {
                if (RegistrationParameters.Password != RegistrationParameters.PasswordConfirm)
                {
                    MatToaster.Add("Passwords Must Match", MatToastType.Warning);
                }
                else
                {
                    var apiResponse = await Http.PostJsonAsync<ApiResponseDto>($"api/Account/AdminUserPasswordReset/{user.UserId}", RegistrationParameters.Password);

                    if (apiResponse.IsSuccessStatusCode)
                        MatToaster.Add("Password Reset", MatToastType.Success, apiResponse.Message);
                    else
                        MatToaster.Add(apiResponse.Message, MatToastType.Danger);

                    IsResetPasswordDialogOpen = false;
                }
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "Password Reset Error");
            }
        }

        public async Task DeleteUserAsync()
        {
            try
            {
                var response = await Http.DeleteAsync($"api/Account/{user.UserId}");

                if (response.StatusCode == (HttpStatusCode)StatusCodes.Status200OK)
                {
                    MatToaster.Add("User Deleted", MatToastType.Success);
                    Users.Remove(user);
                    IsDeleteUserDialogOpen = false;
                    StateHasChanged();
                }
                else
                    MatToaster.Add("User Delete Failed", MatToastType.Danger);
            }
            catch (Exception ex)
            {
                MatToaster.Add(ex.GetBaseException().Message, MatToastType.Danger, "User Delete Error");
            }
        }
    }
}
