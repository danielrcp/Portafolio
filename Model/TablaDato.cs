namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("TablaDato")]
    public partial class TablaDato
    {
        private readonly PortafolioContext context;
        public TablaDato()
        {
            context = new PortafolioContext();
        }
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Relacion { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Valor { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        public int Orden { get; set; }

        public List<TablaDato> Listar(string relacion)
        {
            var Paises = new List<TablaDato>();
            try
            {
                using (context)
                {
                    Paises = context.TablaDato.OrderBy(td => td.Orden)
                                              .Where(td => td.Relacion.Equals(relacion))
                                              .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Paises;
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
