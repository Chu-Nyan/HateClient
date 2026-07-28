using Chu.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.GameSystem
{
    public class EntityContainer
    {
        private readonly BiDictionary<UniqueEntityType, MonoBehaviour> _objectByUniqueType;
        private readonly BiDictionary<string, MonoBehaviour> _objectByFavorite;
        private readonly Dictionary<int, Character> _character;

        public IReadOnlyDictionary<UniqueEntityType, MonoBehaviour> ObjectByUniqueType
        {
            get => _objectByUniqueType;
        }

        public IReadOnlyDictionary<string, MonoBehaviour> ObjectByFavorite
        {
            get => _objectByFavorite;
        }

        public IReadOnlyDictionary<int, Character> Character
        {
            get => _character;
        }

        public EntityContainer()
        {
            _character = new();
            _objectByUniqueType = new();
            _objectByFavorite = new(0);
        }

        public void AddCharacter(Character acter)
        {
            _character.Add(acter.InstanceID, acter);
        }

        public void AddUniqueObject(UniqueEntityType type, MonoBehaviour obj)
        {
            _objectByUniqueType.Add(type, obj);
        }

        public void AddFavoriteObject(string favoriteKey, MonoBehaviour obj)
        {
            _objectByFavorite.Add(favoriteKey, obj);
        }

        public void RemoveCharacter(Character character)
        {
            _character.Remove(character.InstanceID);
            _objectByFavorite.Remove(character);
            _objectByUniqueType.Remove(character);
        }
    }
}
