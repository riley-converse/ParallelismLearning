using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelismLearning
{
    internal class PatternHandler
    {
        private readonly char[] _patterns;

        private int _indexer;

        public int Length => _patterns.Length;

        public PatternHandler(params char[] patterns)
        {
            _patterns = patterns;
            _indexer = 0;
        }

        public (bool, char) GetPattern()
        {
            if (_indexer < _patterns.Length)
            {
                char ch = _patterns[_indexer];
                _indexer++;
                return (true,ch);
            }
            
            return (false,default);
        }

        public void ResetIndexer()
        {
            _indexer = 0;
        }

    }
}
