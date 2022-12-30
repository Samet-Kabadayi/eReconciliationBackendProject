﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class AccountReconciliation:IEntity
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CurrencyAccountId { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public int CurrencyId { get; set; }
        public decimal CurrencyDebit { get; set; }
        public decimal CurrencyCredit { get; set; }
        public bool IsSendEMail { get; set; }
        public DateTime? SendEMailDate { get; set; }
        public bool? IsEMailRead { get; set; }
        public DateTime? EMailReadDate { get; set; }
        public bool? IsResultsucceed  { get; set; }
        public DateTime? ResultDate  { get; set; }
        public string? ResultNote { get; set; }
    }
}