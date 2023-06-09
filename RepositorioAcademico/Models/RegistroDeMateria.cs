//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RepositorioAcademico.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RegistroDeMateria
    {
        public long Id { get; set; }
        public long IdGrupoAperturado { get; set; }
        public short IdEstadoMateriaRegistrada { get; set; }
        public long IdComprobanteRegistro { get; set; }
        public Nullable<short> ItemRecibo { get; set; }
        public Nullable<long> NumeroRecibo { get; set; }
        public Nullable<decimal> PorcentajeBeca { get; set; }
        public Nullable<decimal> Nota { get; set; }
        public Nullable<int> IdBeca { get; set; }
        public Nullable<decimal> Costo { get; set; }
        public int IdMateria { get; set; }
        public Nullable<short> IdPlanEstudio { get; set; }
        public Nullable<long> IdValidezDetalleRecibo { get; set; }
        public Nullable<int> IdNivelDominioCompetencia { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<long> IdSolicitudRegistroEspecial { get; set; }
    
        public virtual ComprobanteRegistro ComprobanteRegistro { get; set; }
        public virtual Materia Materia { get; set; }
    }
}
