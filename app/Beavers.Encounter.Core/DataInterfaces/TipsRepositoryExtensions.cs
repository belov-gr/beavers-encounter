using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beavers.Encounter.Core.DataInterfaces
{
    public static class TipsRepositoryExtensions
    {
        public static int TipPosition(this IEnumerable<Tip> tips, Tip tip)
        {
            int pos = 0;
            foreach (Tip item in tips)
            {
                if (item.Id == tip.Id)
                {
                    return pos;
                }
                pos++;
            }
            return -1;
        }
    }
}
