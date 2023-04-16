using System;
using System.Collections.Generic;
using System.Text;

namespace LangTranslationTelegramBot
{
    class Tutor
    {
        private Dictionary<string, string> _dic = new Dictionary<string, string>();

        WordStorage _storage = new WordStorage();

        private Random _rand = new Random();

        public Tutor()
        {
            _dic = _storage.GetAllWords();
        }

        public bool HasWord(string eng) {
            if (_dic.ContainsKey(eng))
                return true;
            else
                return false;
        }

        public void AddWord(string eng, string rus)
        {
            if (!_dic.ContainsKey(eng))
            {
                _dic.Add(eng, rus);
                _storage.AddWord(eng, rus);
            }
        }

        public bool CheckWord(string eng, string rus)
        {
            var answer = _dic[eng];
            return answer.ToLower() == rus.ToLower();
        }

        public string Translate(string eng)
        {
            if (_dic.ContainsKey(eng))
                return _dic[eng];
            else
                return null;
        }

        public string GetRandomEngWord()
        {
            var r = _rand.Next(0, _dic.Count);
            var keys = new List<string>(_dic.Keys);
            return keys[r];
        }
    }
}
