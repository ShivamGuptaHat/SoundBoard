using SoundBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundBoard.Assets.Models
{
    public class SoundManager
    {
        public static void GetAllSound(ObservableCollection<Sound> sounds)
        {
            var allSound = getSound();
            sounds.Clear();
            allSound.ForEach(p => sounds.Add(p));

        }

        public static void GetSoundsByName(string name,ObservableCollection<Sound> sounds)
        {
            var allsound = getSound();
            var filteredSounds = allsound.Where(p => p.Name == name).ToList();
            filteredSounds.ForEach(p => sounds.Add(p));

        }
         
        public static void GetCategorySounds(string category,ObservableCollection<Sound> sounds)
        {
            var allsounds = getSound();

            var filterSound = allsounds.Where(p => p.Category == category).ToList();
            sounds.Clear();
            filterSound.ForEach(p=>sounds.Add(p));

        }

        private static List<Sound> getSound()
        {
            var Sounds = new List<Sound>();

            Sounds.Add(new Sound("Cow", "Animals"));
            Sounds.Add(new Sound("Cat", "Animals"));

            Sounds.Add(new Sound("Gun", "Cartoons" ));
            Sounds.Add(new Sound("Spring", "Cartoons" ));

            Sounds.Add(new Sound("Clock", "Taunts"  ));
            Sounds.Add(new Sound("LOL", "Taunts"));

            Sounds.Add(new Sound("Ship", "Warnings" ));
            Sounds.Add(new Sound("Siren", "Warnings"));

            return Sounds;
        }
    }
}
