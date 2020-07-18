/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "TypesData", menuName = "Data/TypesData", order = 1)]
    public class TypeDictionary : ScriptableObject
    {
        [SerializeField]
        private List<string> typeName;
        public List<string> TypeName
        {
            get { return typeName; }
        }

        [SerializeField]
        private List<Color> typeColor;
        public List<Color> TypeColor
        {
            get { return typeColor; }
        }

        [SerializeField]
        private List<Sprite> typeIcon;
        public List<Sprite> TypeIcon
        {
            get { return typeIcon; }
        }

        public Color GetColorType(string name)
        {
            for(int i = 0; i < typeName.Count; i++)
            {
                if(typeName[i].Equals(name))
                {
                    return typeColor[i];
                }
            }
            return Color.black;
        }

        public Color GetColorType(int id)
        {
            return typeColor[id];
        }

        public Sprite GetSpriteIcon(int id)
        {
            return typeIcon[id];
        }

        public IEnumerable GetAllTypeName()
        {
            return typeName;//.Select(x => new ValueDropdownItem(x.VariableName, x.VariableName));
        }


        public ValueDropdownList<int> GetAllTypeIndex()
        {
            ValueDropdownList<int> res = new ValueDropdownList<int>();
            for (int i = 0; i < typeName.Count; i++)
                res.Add(new ValueDropdownItem<int>(typeName[i], i));
            return res;
        }
    } 

} // #PROJECTNAME# namespace