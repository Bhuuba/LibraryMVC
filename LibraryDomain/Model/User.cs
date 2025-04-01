using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryDomain.Model
{
    public partial class User : Entity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім!")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Поле не повинно бути порожнім!")]
        [Display(Name = "Електронна пошта")]
        // Регулярний вираз дозволяє лише [a-zA-Z0-9] перед '@'
        // та вимагає закінчення рядка на "gmai,.com"
        [RegularExpression(@"^[a-zA-Z0-9]+@gmail\.com$",
            ErrorMessage = "Email має містити лише літери/цифри перед '@' і закінчуватися на 'gmail.com'")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Поле не повинно бути порожнім!")]
        [Display(Name = "Дата Народження")]
        public DateOnly? Birthdate { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім!")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        // Робимо Status завжди "user" (за замовчуванням)
        [Required(ErrorMessage = "Поле не повинно бути порожнім!")]
        [Display(Name = "Статус")]
        public string? Status { get; set; } = "user";

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Coment> Coments { get; set; } = new List<Coment>();
        public virtual ICollection<Friendship> FriendshipUser1s { get; set; } = new List<Friendship>();
        public virtual ICollection<Friendship> FriendshipUser2s { get; set; } = new List<Friendship>();
        public virtual ICollection<Usergroup> Usergroups { get; set; } = new List<Usergroup>();
    }
}
