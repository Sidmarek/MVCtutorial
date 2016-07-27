using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCtutorial.Admin.Models
{
    public class AdminAddMaskModel
    {
        //Model for add mask to masks of some bakery
        
        [DataType(DataType.Text)]
        [Display(Name = "Id of bakery")]
        public string bakeryId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Name of mask to add")]
        public string maskName { get; set; }

        
        [DataType(DataType.Text)]
        [Display(Name = "Mask permission-role to view and download files:")]
        public string maskRole { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Mask for files selection")]
        public string Mask { get; set; }
    }
    public class AdminRemoveMaskModel
    {
        //Model for remove mask of existing bakery
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Id of bakery")]
        public string bakeryId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name of mask for remove")]
        public string Mask { get; set; }
    }
    public class AdminAddUserModel
    {
        //Model for add user to exsting role
        //It is only admin function so it is not necessary to repeat username

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Username of exsting user")]
        public string User { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Role for upper defined user")]
        public string Role { get; set; }
    }

    public class AdminRemoveUserModel
    {
        //Model for removing user from existing role
        //It is only admin function so it is not necessary to repeat username

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Username of exsting user")]
        public string User { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Role for upper defined user")]
        public string Role { get; set; }
    }
    public class AdminAddRoleModel
    {
        //Model for add role to Roles
        //It is only admin function so it is not necessary to repeat role name

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name of role to add")]
        public string Role { get; set; }
    }

    public class AdminRemoveRoleModel
    {
        //Model for removing role from Roles
        //It is only admin function so it is not necessary to repeat role name

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name of removing role")]
        public string Role { get; set; }
    }
}