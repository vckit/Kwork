//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AppZero.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpareParts
    {
        public int ID { get; set; }
        public string RackNumber { get; set; }
        public string ShelfNumber { get; set; }
        public string Description { get; set; }
        public int IDTypeObject { get; set; }
        public int Count { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual TypeObject TypeObject { get; set; }
    }
}
