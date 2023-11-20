using EdgeProject.Core.Entities.Order_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentSpecification:BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string intentId)
            :base(O=>O.PaymentIntentId == intentId )
        {
            
        }

    }
}
