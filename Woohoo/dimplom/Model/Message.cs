//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dimplom.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public int RecipentId { get; set; }
        public string Messege { get; set; }
        public System.DateTime DataSend { get; set; }
    
        public virtual Chats Chats { get; set; }
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
    }
}