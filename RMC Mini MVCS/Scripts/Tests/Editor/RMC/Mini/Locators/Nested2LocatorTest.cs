using NUnit.Framework;
using UnityEngine;
using System;

namespace RMC.Mini.Locators
{
    public interface IA
    {
    }

    public class SampleA : IA
    {
    }

    public class SampleSubA : SampleA
    {
    }

    public class SampleSubASubB : SampleSubA
    {
    }

    public interface ILocatorHolder
    {
        Locator<IA> MyLocator { get; }
    }

    public class SampleInnerClass : ILocatorHolder
    {
        public Locator<IA> MyLocator { get; private set; }

        public SampleInnerClass()
        {
            MyLocator = new Locator<IA>();
            Debug.Log($"SampleInnerClass created with MyLocator of type {MyLocator.GetType().FullName}");
        }
    }

    public class SampleOuterClassGeneric<T> where T : class, IA
    {
        private readonly ILocatorHolder _locatorHolder;

        public Locator<T> MyLocator => _locatorHolder.MyLocator.GetOrCreateLocator<T>();

        public SampleOuterClassGeneric(ILocatorHolder locatorHolder)
        {
            _locatorHolder = locatorHolder;
        }
    }

    [Category("RMC.Mini.Locators")]
    public class Nested2LocatorTest
    {
        [Test]
        public void MyLocator_ShouldReturnCorrectLocator_WhenInitialized()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleA>(inner);

            var myLocator = outer.MyLocator;
            Assert.IsNotNull(myLocator, "MyLocator should not be null");
        }

        [Test]
        public void AddItem_ShouldAddItemToMyLocator_WhenItemIsAdded()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleA>(inner);

            var item = new SampleA();
            var myLocator = outer.MyLocator;
            Assert.IsNotNull(myLocator, "MyLocator should not be null");

            myLocator.AddItem(item);

            Assert.IsTrue(myLocator.HasItem<SampleA>(), "MyLocator should have the item");
        }

        [Test]
        public void GetItem_ShouldReturnItemFromMyLocator_WhenItemIsAdded()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleA>(inner);

            var item = new SampleA();
            var myLocator = outer.MyLocator;
            Assert.IsNotNull(myLocator, "MyLocator should not be null");

            myLocator.AddItem(item);

            var retrievedItem = myLocator.GetItem<SampleA>();
            Assert.AreSame(item, retrievedItem, "Retrieved item should be the same as the added item");
        }

        [Test]
        public void MyLocator_ShouldReturnNullForInvalidCast_WhenTypeMismatchOccurs()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleSubA>(inner);
            var item = new SampleA();
            inner.MyLocator.AddItem(item);
            var myLocator = outer.MyLocator;

            if (myLocator == null)
            {
                Debug.Log("MyLocator is null as expected for invalid cast.");
            }
            else
            {
                Debug.LogError("MyLocator should be null for invalid cast, but it is not.");
            }

            Assert.IsNull(myLocator, "MyLocator should be null for invalid cast");
        }

        [Test]
        public void AddItem_ShouldThrowExceptionForDuplicateWithoutKey_WhenDuplicateItemIsAdded()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleA>(inner);

            var item1 = new SampleA();
            var item2 = new SampleA();
            var myLocator = outer.MyLocator;
            Assert.IsNotNull(myLocator, "MyLocator should not be null");

            myLocator.AddItem(item1);
            Assert.Throws<Exception>(() => myLocator.AddItem(item2), "Adding a duplicate item without a key should throw an exception");
        }

        [Test]
        public void AddItem_WithDifferentKeys_ShouldAddItems_WhenItemsWithDifferentKeysAreAdded()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleA>(inner);

            var item1 = new SampleA();
            var item2 = new SampleA();
            var myLocator = outer.MyLocator;
            Assert.IsNotNull(myLocator, "MyLocator should not be null");

            myLocator.AddItem(item1, "key1");
            myLocator.AddItem(item2, "key2");

            Assert.IsTrue(myLocator.HasItem<SampleA>("key1"), "MyLocator should have the item with key 'key1'");
            Assert.IsTrue(myLocator.HasItem<SampleA>("key2"), "MyLocator should have the item with key 'key2'");
        }

        [Test]
        public void RemoveItem_ShouldRemoveItemFromMyLocator_WhenItemIsRemoved()
        {
            var inner = new SampleInnerClass();
            var outer = new SampleOuterClassGeneric<SampleA>(inner);

            var item = new SampleA();
            var myLocator = outer.MyLocator;
            Assert.IsNotNull(myLocator, "MyLocator should not be null");

            myLocator.AddItem(item);
            myLocator.RemoveItem<SampleA>();

            Assert.IsFalse(myLocator.HasItem<SampleA>(), "MyLocator should not have the item after removal");
        }
    }
}
