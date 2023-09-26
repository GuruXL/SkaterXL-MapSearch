using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MapSearch.Utils
{
    public class SearchFilter : MonoBehaviour
    {
        // public static SearchFilter __instance { get; private set; }
        // public static SearchFilter Instance => __instance ?? (__instance = new SearchFilter());

        private List<LevelInfo> LevelList;
        private List<LevelInfo> ModLevelList;
        private List<LevelInfo> CommunityLevelList;

        public List<LevelInfo> CombinedMapList = new List<LevelInfo>();
        public string[] Mapnames;

        public void GetMaps()
        {
            CombinedMapList.Clear();

            LevelList = LevelManager.Instance.Levels?.ToList();
            ModLevelList = LevelManager.Instance.ModLevels?.ToList();
            CommunityLevelList = LevelManager.Instance.CommunityLevels?.ToList();

            if (LevelList != null)
            {
                CombinedMapList.AddRange(LevelList);
            }

            if (ModLevelList != null)
            {
                CombinedMapList.AddRange(ModLevelList);
            }

            if (CommunityLevelList != null)
            {
                CombinedMapList.AddRange(CommunityLevelList);
            }

            Mapnames = ConvertToString(CombinedMapList);
        }

        public string[] ConvertToString(List<LevelInfo> info)
        {
            string[] names = new string[info.Count];

            for(int i = 0; i < info.Count; i++)
            {
                names[i] = info[i].name;
            }
            return names;
        }

        public string[] FilterArray(string[] filteredStrings, string searchstring)
        {
            string[] searchWords = searchstring.ToLower().Split(' ');

            return filteredStrings = Mapnames.Where(s =>
            {
                bool containsAllWords = true;
                foreach (string searchWord in searchWords)
                {
                    if (!s.ToLower().Contains(searchWord))
                    {
                        containsAllWords = false;
                        break;
                    }
                }
                return containsAllWords;
            }).ToArray();
        }
    }
}
