namespace WebApplication2.Models.DTOs
{
    public class LoginDTO
    {
        /// <summary>
        /// O endereço de e-mail do usuário.
        /// </summary>
        /// <example>Example@example.com</example>
        public string Email { get; set; }

        /// <summary>
        /// A senha do usuário (mínimo de 8 caracteres).
        /// </summary>
        /// <example>Senha@123</example>
        public string Password { get; set; }



    }
}
