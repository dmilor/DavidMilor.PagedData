using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DavidMilor.PagedData.Tests
{
    public class ExtensionMethods
    {
        const int PAGESIZE_VALID = 10;
        const int PAGESIZE_INVALID = 0;

        #region MaximumPage


        //[Fact]
        //public void MaximumPage_EmptyQuery_CorrectResult()
        //{
        //    var query = ValidQuery(0);

        //    Assert.Equal(0, query.MaximumPage(PAGESIZE_VALID));
        //}

        //[Fact]
        //public void MaximumPage_QueryHasOneIncompletePage_CorrectResult()
        //{
        //    var query = ValidQuery(PAGESIZE_VALID-1);

        //    Assert.Equal(1, query.MaximumPage(PAGESIZE_VALID));
        //}

        //[Fact]
        //public void MaximumPage_QueryHasExactlyOnePage_CorrectResult()
        //{
        //    var query = ValidQuery(PAGESIZE_VALID);

        //    Assert.Equal(1, query.MaximumPage(PAGESIZE_VALID));
        //}
        //[Fact]
        //public void MaximumPage_QueryHasFullAndPartialPage_CorrectResult()
        //{
        //    var query = ValidQuery(PAGESIZE_VALID+1);

        //    Assert.Equal(2, query.MaximumPage(PAGESIZE_VALID));
        //}

        //[Fact]
        //public void MaximumPage_QueryHasThreeFullPages_CorrectResult()
        //{
        //    var query = ValidQuery(PAGESIZE_VALID * 3);

        //    Assert.Equal(3, query.MaximumPage(PAGESIZE_VALID));
        //}
        #endregion MaximumPage

        #region Page - parameter checking
        [Fact]
        public void Page_NullQuery_ThrowsException()
        {
            IOrderedQueryable<int> query = null;

            Assert.Throws<ArgumentNullException>(()=>query.Page(1,PAGESIZE_VALID));
        }

        [Fact]
        public void Page_PageSizeLessThanOne_ThrowsException()
        {
            IOrderedQueryable<int> query = ValidQuery(20);

            Assert.Throws<ArgumentOutOfRangeException>(() => query.Page(1, 0));
        }

        [Fact]
        public void Page_PageLessThanOne_ThrowsException()
        {
            IOrderedQueryable<int> query = ValidQuery(20);

            Assert.Throws<ArgumentOutOfRangeException>(() => query.Page(0, 1));
        }
        #endregion Page - parameter checking

        #region Page - result checking
        [Fact]
        public void Page_NoDataInSet_ReturnsCorrectly()
        {
            var query = ValidQuery(0);

            var result = query.Page(1, PAGESIZE_VALID);

            Assert.NotNull(result);

            Assert.Equal(0, result.Page);
            Assert.Equal(0, result.MaximumPage);
            Assert.Equal(0, result.PageSize);
            Assert.Equal(0, result.FullSetAmount);

            Assert.Null(result.Items);

            Assert.False(result.IsFirst);
            Assert.False(result.IsLast);
            Assert.False(result.HasPrevious);
            Assert.False(result.HasNext);

        }

        [Fact]
        public void Page_DataInSet_FirstPage_ReturnsCorrectly()
        {
            var query = DefaultQuery();

            var result = query.Page(1, PAGESIZE_VALID);

            Assert.NotNull(result);

            Assert.Equal(1, result.Page);
            Assert.Equal(3, result.MaximumPage);
            Assert.Equal(PAGESIZE_VALID, result.PageSize);
            Assert.Equal(DefaultQuerySize(), result.FullSetAmount);

            Assert.NotNull(result.Items);
            Assert.Equal(PAGESIZE_VALID, result.Items.Count());
            Assert.Equal(1, result.Items.First());
            Assert.Equal(10, result.Items.Last());

            Assert.True(result.IsFirst);
            Assert.False(result.IsLast);
            Assert.False(result.HasPrevious);
            Assert.True(result.HasNext);

        }

        [Fact]
        public void Page_DataInSet_SecondPage_ReturnsCorrectly()
        {
            var query = DefaultQuery();

            var result = query.Page(2, PAGESIZE_VALID);

            Assert.NotNull(result);

            Assert.Equal(2, result.Page);
            Assert.Equal(3, result.MaximumPage);
            Assert.Equal(PAGESIZE_VALID, result.PageSize);
            Assert.Equal(DefaultQuerySize(), result.FullSetAmount);

            Assert.NotNull(result.Items);
            Assert.Equal(PAGESIZE_VALID, result.Items.Count());
            Assert.Equal(11, result.Items.First());
            Assert.Equal(20, result.Items.Last());

            Assert.False(result.IsFirst);
            Assert.False(result.IsLast);
            Assert.True(result.HasPrevious);
            Assert.True(result.HasNext);

        }
        [Fact]
        public void Page_DataInSet_LastPage_ReturnsCorrectly()
        {
            var query = DefaultQuery();

            var result = query.Page(3, PAGESIZE_VALID);

            Assert.NotNull(result);

            Assert.Equal(3, result.Page);
            Assert.Equal(3, result.MaximumPage);
            Assert.Equal(PAGESIZE_VALID, result.PageSize);
            Assert.Equal(DefaultQuerySize(), result.FullSetAmount);

            Assert.NotNull(result.Items);
            Assert.Equal(9, result.Items.Count());
            Assert.Equal(21, result.Items.First());
            Assert.Equal(29, result.Items.Last());

            Assert.False(result.IsFirst);
            Assert.True(result.IsLast);
            Assert.True(result.HasPrevious);
            Assert.False(result.HasNext);

        }
        #endregion Page - result checking



        #region helpers
        private IOrderedQueryable<int> DefaultQuery()
        {
            return ValidQuery(DefaultQuerySize());
        }
        private int DefaultQuerySize()
        {
            //two full pages and a partial page
            return (PAGESIZE_VALID * 3) - 1;
        }

        private IOrderedQueryable<int> ValidQuery(int amountOfRecordsInQuery)
        {
            if(amountOfRecordsInQuery <= 0)
            {
                return new List<int>().AsQueryable().OrderBy(n=>n);
            }   
            else
            {
                return Enumerable.Range(1, amountOfRecordsInQuery).ToList().AsQueryable().OrderBy(n=>n);
            }
        }
        #endregion helpers
    }
}