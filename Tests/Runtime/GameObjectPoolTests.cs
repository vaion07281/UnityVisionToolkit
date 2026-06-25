using NUnit.Framework;
using UnityEngine;
using UnityVisionToolkit.Runtime;

namespace UnityVisionToolkit.Tests.Runtime
{
    public class GameObjectPoolTests
    {
        private GameObject _prefab;
        private GameObjectPool _pool;

        [SetUp]
        public void SetUp()
        {
            var poolObj = new GameObject("Pool");
            _pool = poolObj.AddComponent<GameObjectPool>();

            _prefab = new GameObject("Prefab");
            _pool.Prefab = _prefab;
            _pool.DefaultCapacity = 10;
            _pool.MaxSize = 20;

            // Trigger Awake manually for testing if needed after setting properties
            // Note: AddComponent automatically calls Awake, but since properties are set after,
            // the manual invocation here re-initializes the pool with the correct prefab.
            _pool.GetType().GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(_pool, null);
        }

        [TearDown]
        public void TearDown()
        {
            if (_pool != null) GameObject.DestroyImmediate(_pool.gameObject);
            if (_prefab != null) GameObject.DestroyImmediate(_prefab);
        }

        [Test]
        public void GameObjectPool_Get_ReturnsInstance()
        {
            // Act
            var instance = _pool.Get();

            // Assert
            Assert.IsNotNull(instance);
            Assert.IsTrue(instance.activeSelf);
        }

        [Test]
        public void GameObjectPool_Release_DeactivatesInstance()
        {
            // Arrange
            var instance = _pool.Get();

            // Act
            _pool.Release(instance);

            // Assert
            Assert.IsFalse(instance.activeSelf);
        }
    }
}