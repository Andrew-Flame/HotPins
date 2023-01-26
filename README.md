<div align=center>
    <img src='img/SocialCard.jpg' width=75% href='google.com'/>
    <br>
    <a href='https://valheim.thunderstore.io/package/Flame/HotPins'>
        <img src='https://img.shields.io/endpoint?label=Thunderstore%20downloads&style=for-the-badge&url=https://valheim-modtracker.vercel.app/thunderstore/Flame/HotPins/downloads'/>
        <img src='https://img.shields.io/endpoint?label=Thunderstore%20thumbsups&style=for-the-badge&url=https://valheim-modtracker.vercel.app/thunderstore/Flame/HotPins/ratings'/>
    </a>
    <br>
    <a href='https://www.nexusmods.com/valheim/mods/2228'>
        <img src='https://img.shields.io/endpoint?label=Nexusmods%20views&style=for-the-badge&url=https://valheim-modtracker.vercel.app/nexusmods/2228/views'/>
        <img src='https://img.shields.io/endpoint?label=Nexusmods%20endorsed&style=for-the-badge&url=https://valheim-modtracker.vercel.app/nexusmods/2228/endorsed'/>
    </a>
    <br>
    <a href='https://github.com/Andrew-Flame/HotPins/releases/latest'><img src='https://img.shields.io/github/v/release/Andrew-Flame/HotPins?style=for-the-badge&color=0cd94e'/></a>
    <img src='https://img.shields.io/github/repo-size/Andrew-Flame/HotPins?style=for-the-badge&color=0cd94e'/>
    <a href='https://github.com/Andrew-Flame/HotPins/blob/master/LICENSE'><img src='https://img.shields.io/github/license/Andrew-Flame/HotPins?style=for-the-badge&color=0cd94e'/></a>
</div>

---
### Description
'Hot Pins' is a mod for Valheim that adds ways to quickly add markers to the game map.
Have you ever found ore deposits or dungeons while running away from enemies?
What were you doing at that moment?
Maybe you tried to set a pin on the map, risking death, asked your friend to mark this place or just did nothing.
Especially for such cases, I have developed this modification.
Now you can create pins on the game map completely safely in the blink of an eye.
All you need after installing my mod is to use standard keys and keyboard shortcuts (or customize your own) to create pins.

### Downloading
<!--
<a href='https://www.nexusmods.com/valheim/mods/2228'>
    <img src='https://user-images.githubusercontent.com/82677442/214826952-b9ad0959-501b-43b0-9ea5-1df607a5320d.svg' height=75px/>
</a>
<a href='https://github.com/Andrew-Flame/HotPins/releases/latest'>
    <img src='https://user-images.githubusercontent.com/82677442/214825167-8f182e52-ddef-40cc-a55d-509e58494d05.jpg' height=75px/>
</a>
-->

You can download this mod from:
- [Thunderstore](https://valheim.thunderstore.io/package/Flame/HotPins)
- [Nexusmods](https://www.nexusmods.com/valheim/mods/2228)
- [GitHub Releases](https://github.com/Andrew-Flame/HotPins/releases/latest)

### Default keyboard shortcuts
- Keypad1 = "Hammer" "Burial Chambers"
- Keypad2 = "Hammer" "Troll Cave"
- Keypad3 = "Hammer" "Sunken Crypts"
- Keypad4 = "Hammer" "Frost Cave"
- Keypad5 = "Hammer" "Fuling Village"
- Keypad6 = "Hammer" "Infested mine"
- LeftAlt+Keypad1 = "Ball" "Copper"
- LeftAlt+Keypad2 = "Ball" "Tin"
- LeftAlt+Keypad3 = "Ball" "Guck"
- LeftAlt+Keypad4 = "Ball" "Silver"
- LeftAlt+Keypad5 = "Ball" "Tar"
- LeftAlt+Keypad6 = "Ball" "Marble"

### Manual
1. Unpack the downloaded archive
2. Move the file `HotPins.dll` to the `Valheim/BepInEx/plugins` folder
3. Run the game, it will generate automatically an configuration file into `Valheim/BepInEx/config`

### Customization
- After the first launch of the mod, a config file `Flame.HotPins.cfg` will be created in the `Valheim/BepInEx/config` folder.
- Next, you need to press the bind, you need to make a record of the form: 
- `UnityKeyCode = "Pin Type" "Pin Name"` or `UnityKeyCode1+UnityKeyCode2 = "Pin Type" "Pin Name"` (you can use keyboard shortcuts from multiple keys)
- You can view all [UnityKeyCodes here](https://t.ly/dMe2)
- Available pin types: Fireplace, House, Hammer, Ball, Cave
- As the pin name you can use any string
