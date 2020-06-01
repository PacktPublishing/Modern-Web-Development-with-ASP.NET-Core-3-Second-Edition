using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class MyTests : IClassFixture<SharedData>
    {
        private readonly SharedData _data;

        public MyTests(SharedData data)
        {
            this._data = data;
        }

        [Fact(Skip = "Not implemented yet")]
        public void TestSomething()
        {
            //will be ignored
        }
    }
}
