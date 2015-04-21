using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextExtraction.Extract {
    public interface ITextExtractor {

        string Tag { get; set; }

        string FindNextContent();
        bool HasNextContent();
    }
}
