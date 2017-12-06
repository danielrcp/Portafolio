namespace Model
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("Usuario")]
    public partial class Usuario
    {
        private readonly PortafolioContext context;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Experiencia = new HashSet<Experiencia>();
            Habilidad = new HashSet<Habilidad>();
            Testimonio = new HashSet<Testimonio>();
            context = new PortafolioContext();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [Column(TypeName = "text")]
        public string Direccion { get; set; }

        [StringLength(50)]
        public string Ciudad { get; set; }

        public int? Pais_id { get; set; }

        [StringLength(50)]
        public string Telefono { get; set; }

        [StringLength(100)]
        public string Facebook { get; set; }

        [StringLength(100)]
        public string Twitter { get; set; }

        [StringLength(100)]
        public string YouTube { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Experiencia> Experiencia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Habilidad> Habilidad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Testimonio> Testimonio { get; set; }


        /// <summary>
        /// Valida usuario en base de datos
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResponseModel Acceder(string email, string password)
        {
            var rm = new ResponseModel();
            string clave = HashHelper.MD5(password);
            try
            {
                using (context)
                {
                    var usuario = context.Usuario.Where(u => u.Email.Equals(email) && u.Password.Equals(clave)).SingleOrDefault();
                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.id.ToString());
                        rm.SetResponse(true);
                    }
                    else
                    {
                        rm.SetResponse(false,"Correo o contrase�a incorrecta");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return rm;
        }

        /// <summary>
        /// Obtiene usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario Obtener(int id)
        {
            var Usuario = new Usuario();
            try
            {
                using (context)
                {
                    Usuario = context.Usuario.Find(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Usuario;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <author></author>
        /// <datetime></datetime>
        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

    }
}
