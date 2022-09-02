using System.Collections.Generic;
using System.Linq;
using GameScripts.Characters;
using GameScripts.Game;
using GameScripts.Level;
using GameScripts.Misc;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.Bot
{
    public class BotSpawner : MonoBehaviour
    {
        [SerializeField] private List<Material> colors;

        private List<BotSpawnPoint> _botSpawns;
        private GameStarter _gameStarter;
        private BotStateFactory _botStateFactory;
        private TrackPositionsViewModel _trackPositionsViewModel;

        private readonly List<string> _botNames = new List<string>
        {
            "Aaron", "Abraham", "Adam", "Adrian", "Aidan", "Alan", "Albert", "Alejandro", "Alex", "Alexander", "Alfred",
            "Andrew", "Angel", "Anthony", "Antonio", "Ashton", "Austin",
            "Benjamin", "Bernard", "Blake", "Brandon", "Brian", "Bruce", "Bryan",
            "Cameron", "Carl", "Carlos", "Charles", "Christopher", "Cole", "Connor", "Caleb", "Carter", "Chase",
            "Christian", "Clifford", "Cody", "Colin", "Curtis", "Cyrus",
            "Daniel", "David", "Dennis", "Devin", "Diego", "Dominic", "Donald", "Douglas", "Dylan",
            "Edward", "Elijah", "Eric", "Ethan", "Evan",
            "Francis", "Fred",
            "Gabriel", "Gavin", "Geoffrey", "George", "Gerld", "Gilbert", "Gordon", "Graham", "Gregory",
            "Harold", "Harry", "Hayden", "Henry", "Herbert", "Horace", "Howard", "Hugh", "Hunter",
            "Ian", "Isaac", "Isaiah",
            "Jack", "Jackson", "Jacob", "Jaden", "Jake", "James", "Jason", "Jayden", "Jeffery", "Jeremiah", "Jesse",
            "Keith", "Kevin", "Kyle",
            "Landon", "Lawrence", "Leonars", "Lewis", "Logan", "Louis", "Lucas", "Luke",
            "Malcolm", "Martin", "Mason", "Matthew", "Michael", "Miguel", "Miles", "Morgan",
            "Nathan", "Nathaniel", "Neil", "Nicholas", "Noah", "Norman",
            "Oliver", "Oscar", "Oswald", "Owen",
            "Patrick", "Peter", "Philip",
            "Ralph", "Raymond", "Reginald", "Richard", "Robert", "Rodrigo", "Roger", "Ronald", "Ryan",
            "Samuel", "Sean", "Sebastian", "Seth", "Simon", "Stanley", "Steven",
            "Thomas", "Timothy", "Tyler",
            "Wallace", "Walter", "William", "Wyatt",
            "Xavier",
            "Zachary"
        };

        [Inject]
        private void Constructor(
            GameStarter gameStarter,
            BotStateFactory botStateFactory,
            TrackPositionsViewModel trackPositionsViewModel)
        {
            _trackPositionsViewModel = trackPositionsViewModel;
            _botStateFactory = botStateFactory;
            _gameStarter = gameStarter;
            _gameStarter.State
                .Where(s => s == GameState.Ready)
                .Subscribe(_ => Spawn())
                .AddTo(this);
        }

        private void Spawn()
        {
            _botSpawns = FindObjectsOfType<BotSpawnPoint>().ToList();
            DestroyAllBots(_botSpawns);

            for (var i = 0; i < _botSpawns.Count; i++)
            {
                var point = _botSpawns[i];
                var pointTransform = point.transform;
                var bot = _botStateFactory.Create();
                var botTransform = bot.transform;

                botTransform.position = pointTransform.position;
                botTransform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                botTransform.SetParent(pointTransform);

                bot.GetComponentInChildren<SkinnedMeshRenderer>().material = colors[i];
                var botName = bot.GetComponentInChildren<TextMeshProUGUI>();
                botName.text = _botNames[Random.Range(0, _botNames.Count - 1)];
                botName.color = colors[i].color;

                var triangle = bot.GetComponentInChildren<Image>();
                triangle.color = colors[i].color;

                var offset = bot.GetComponent<CharacterOffset>();
                offset.ChangeOffset(i);

                _trackPositionsViewModel.AddBot(botTransform);
            }
        }

        private void DestroyAllBots(List<BotSpawnPoint> spawns)
        {
            foreach (var spawnPoint in spawns)
            {
                var point = spawnPoint.transform;
                if (point.childCount == 0)
                {
                    continue;
                }

                _trackPositionsViewModel.RemoveBot(point.GetChild(0));
                point.DestroyAllChildren();
            }
        }
    }
}