using skcyDMSCataloguing.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.Services
{
    public interface IGetObjectType
    {
        string GetObjectName<T>() where T : class;
    }

    public class GetObjectType : IGetObjectType
    {
        private readonly AppDbContext context;

        public GetObjectType(AppDbContext context)
        {
            this.context = context;
        }

        public string GetObjectName<T>() where T : class
        {
            var getobjecttype = context.Set<T>().GetType().ToString();
            //character's "." representation  is "\u002E" 
            int lastoccurenceofchar = getobjecttype.LastIndexOf("\u002E", getobjecttype.Length - 1, getobjecttype.Length);
            string cat = getobjecttype.Substring(lastoccurenceofchar + 1, (getobjecttype.Length - lastoccurenceofchar) - 2) ?? "none";
            return cat;
        }
    }
}
