using SFMS.Models;
using System.Collections.Generic;

namespace SFMS.Repository {
    public class BatchRepository : IBatchRepository {
        public void Create(Batch model) {
          
        }

        public void Delete(string id) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Batch> ReteriveActive() {
            throw new System.NotImplementedException();
        }

        public void Update(Batch model) {
            throw new System.NotImplementedException();
        }
    }
}
