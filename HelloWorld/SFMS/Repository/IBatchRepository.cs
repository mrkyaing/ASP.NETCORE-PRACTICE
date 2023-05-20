using SFMS.Models;
using System.Collections.Generic;

namespace SFMS.Repository {
    public interface IBatchRepository {
        void Create(Batch model);
        IEnumerable<Batch> ReteriveActive();
        void Update(Batch model);
        void Delete(string id);
    }
}
