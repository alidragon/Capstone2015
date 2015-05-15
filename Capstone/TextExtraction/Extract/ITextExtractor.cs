using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextExtraction.Extract {
    public interface ITextExtractor: IEnumerator<string> {
        string Tag { get; set; }

        string FindNextContent();
        bool HasNextContent();
    }
}
