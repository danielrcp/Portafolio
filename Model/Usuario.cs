namespace Model
{
    using Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Spatial;
    using System.Data.Entity.Validation;
    using System.IO;
    using System.Linq;
    using System.Web;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Experiencia = new HashSet<Experiencia>();
            Habilidad = new HashSet<Habilidad>();
            Testimonio = new HashSet<Testimonio>();
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
                using (var ctx = new PortafolioContext())
                {
                    var usuario = ctx.Usuario.Where(u => u.Email.Equals(email) && u.Password.Equals(clave)).SingleOrDefault();
                    if (usuario != null)
                    {
                        SessionHelper.AddUserToSession(usuario.id.ToString());
                        rm.SetResponse(true);
                    }
                    else
                    {
                        rm.SetResponse(false,"Correo o contraseña incorrecta");
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
                using (var ctx = new PortafolioContext())
                {
                    Usuario = ctx.Usuario.Find(id);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Usuario;
        }


        public ResponseModel Guardar(HttpPostedFileBase Foto)
        {
            var rm = new ResponseModel();
            try
            {
                using (var ctx = new PortafolioContext())
                {
                    ctx.Configuration.ValidateOnSaveEnabled = false;

                    var eUsuario = ctx.Entry(this);
                    eUsuario.State = EntityState.Modified;

                    // Campos que queremos ignorar
                    if (Foto != null)
                    {
                        // Nombre del archivo, es decir, lo renombramos para que no se repita nunca
                        string archivo = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(Foto.FileName);

                        // La ruta donde lo vamos guardar
                        Foto.SaveAs(HttpContext.Current.Server.MapPath("~/uploads/" + archivo));

                        // Establecemos en nuestro modelo el nombre del archivo
                        this.Foto = archivo;
                    }
                    else eUsuario.Property(x => x.Foto).IsModified = false;

                    if (this.Password == null) eUsuario.Property(x => x.Password).IsModified = false;

                    ctx.SaveChanges();

                    rm.SetResponse(true);
                }
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
           
            catch (Exception e)
            {
                throw;
            }
            return rm;
        }

    }
}
