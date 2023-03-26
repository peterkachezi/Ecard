using System;

namespace Ecard.Models
{
    public class EcardDetail
    {
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        public DateTime CreateDate { get; set; }
        public byte[] PassportPhoto { get; set; }


    }
}