using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Boilerplate.Tests
{
    public class temp
    {
        [Fact]
        public void test()
        {
            var a = (int)'a';
            var Z = (int)'Z';
            if ('a' < 'Z')
            {

            }

            Regex regex = new Regex(@"^[A-Za-z0-9._\-\[\]]+$");
            if (regex.IsMatch(""))
            {

            }
        }
    }
}
