using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Tree.IO {
    public class InvalidFileException : Exception {
        public InvalidFileException()
            : base() {

        }

        public InvalidFileException(string message)
            : base(message) {

        }
    }
}
