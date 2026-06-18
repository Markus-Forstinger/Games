# BoardTest – Avalonia Board für C#-Projekte

Dieses Projekt stellt dir ein **visuelles Rasterfenster** zur Verfügung, das du in deinem C#-Konsolenprogramm beschreiben und auslesen kannst — ohne GUI-Kenntnisse.  
Du bearbeitest nur eine einzige Datei: **`BoardDemo.cs`**.

---

## So sieht das Board aus

```
     0    1    2    3    4
 0 [    ][    ][    ][    ][    ]
 1 [    ][    ][    ][    ][    ]
 2 [    ][ R  ][ G  ][ S  ][    ]
 3 [    ][    ][ X  ][    ][    ]
    ...
```

Jede Zelle wird durch eine **Zeile** (oben = 0) und eine **Spalte** (links = 0) angegeben.

---

## Projektstruktur

| Datei | Bedeutung | Anfassen? |
|-------|-----------|-----------|
| `BoardDemo.cs` | **Hier kommt dein Code rein** | ✅ Ja |
| `Program.cs` | Startet Avalonia + deinen Logik-Thread | ❌ Nein |
| `Board.cs` | Implementierung der Board-API | ❌ Nein |
| `BoardWindow.cs` | Das Avalonia-Fenster | ❌ Nein |
| `App.cs` | Avalonia-Bootstrap | ❌ Nein |

---

## Board-API – alle Methoden, die du brauchst

### Initialisieren (zuerst aufrufen!)

```csharp
Board.Init(rows, cols, title);
Board.Init(rows, cols, title, cellWidth, cellHeight, fontSize);
```

Öffnet das Board-Fenster mit der angegebenen Anzahl an Zeilen und Spalten.  
`cellWidth`, `cellHeight` und `fontSize` sind optional — die Standardwerte erzeugen kleine Zellen, die für große Raster geeignet sind.

| Parameter | Standard | Bedeutung |
|-----------|----------|-----------|
| `cellWidth` | `36` | Breite jeder Datenzelle in Pixeln |
| `cellHeight` | `24` | Höhe jeder Datenzelle in Pixeln |
| `fontSize` | `11` | Schriftgröße des Textes in den Zellen |

```csharp
Board.Init(5, 5, "Mein Board");                                  // kleine Standardzellen
Board.Init(5, 5, "Mein Board", cellWidth: 60, cellHeight: 48);   // größere Zellen
Board.Init(3, 3, "Groß",       cellWidth: 80, cellHeight: 70, fontSize: 32);  // TicTacToe-Stil
```

---

### In eine Zelle schreiben

```csharp
Board.SetText(row, col, text);                 // schwarzer Text
Board.SetText(row, col, text, color);          // farbiger Text
```

`color` muss einer dieser Werte sein: `"Black"`, `"Red"`, `"Green"`, `"Gray"`

```csharp
Board.SetText(0, 0, "X");              // schwarzes X in der oberen linken Zelle
Board.SetText(1, 2, "O", "Red");       // rotes O in Zeile 1, Spalte 2
Board.SetText(3, 3, "?", "Green");     // grünes ? in Zeile 3, Spalte 3
```

---

### Aus einer Zelle lesen

```csharp
string text = Board.GetText(row, col);
```

Gibt den aktuellen Text der Zelle zurück, oder `""` wenn die Zelle leer ist.

```csharp
string inhalt = Board.GetText(1, 2);    // liest den Inhalt von Zeile 1, Spalte 2
if (inhalt == "X")
{
    Console.WriteLine("Ein X gefunden!");
}
```

---

### Board löschen

```csharp
Board.Clear();    // leert alle Zellen
```

---

### Boardgröße abfragen

```csharp
int zeilen  = Board.GetLength(0);   // Anzahl der Zeilen
int spalten = Board.GetLength(1);   // Anzahl der Spalten
```

Nützlich, wenn du alle Zellen durchlaufen möchtest:

```csharp
for (int r = 0; r < Board.GetLength(0); r++)
{
    for (int c = 0; c < Board.GetLength(1); c++)
    {
        Board.SetText(r, c, $"{r},{c}");
    }
}
```

---

### Auf einen Klick warten

```csharp
var (row, col) = Board.WaitForClick();
```

**Hält dein Programm an**, bis der Benutzer auf eine Zelle im Board-Fenster klickt.  
Gibt die Zeile und Spalte der angeklickten Zelle zurück.

```csharp
Console.WriteLine("Klicke auf eine beliebige Zelle...");
var (row, col) = Board.WaitForClick();
Console.WriteLine($"Du hast Zeile {row}, Spalte {col} angeklickt.");
Board.SetText(row, col, "✓", "Green");
```

> **Hinweis:** `var (row, col) = ...` nennt man *Tupel-Destrukturierung* — damit werden zwei Werte gleichzeitig ausgepackt.  
> Du kannst es auch in zwei Zeilen schreiben:
> ```csharp
> (int row, int col) = Board.WaitForClick();
> ```

---

### Board schließen

```csharp
Board.Exit();   // schließt das Fenster und beendet das Programm
```

Diesen Aufruf immer ganz am Ende deiner `Run()`-Methode einfügen.

---

## Programm starten

```
dotnet run --project BoardTest-avalonia.csproj
```

---

## Eigene Logik schreiben

Öffne **`BoardDemo.cs`** und ersetze (oder erweitere) den Inhalt von `Run()`:

```csharp
public static void Run()
{
    Board.Init(5, 5, "Mein Programm");

    // dein Code hier ...

    Board.Exit();
}
```

Alles zwischen `Board.Init(...)` und `Board.Exit()` gehört dir.

---

## Beispiel 1 – Board mit Koordinaten füllen

```csharp
public static void Run()
{
    Board.Init(4, 4, "Koordinaten");

    for (int r = 0; r < Board.GetLength(0); r++)
    {
        for (int c = 0; c < Board.GetLength(1); c++)
        {
            Board.SetText(r, c, $"{r},{c}");
        }
    }

    Console.ReadLine();
    Board.Exit();
}
```

---

## Beispiel 2 – Zellen per Klick markieren und wieder löschen

```csharp
public static void Run()
{
    Board.Init(4, 4, "Markieren");

    Console.WriteLine("Klicke auf Zellen, um sie zu markieren. Enter zum Beenden.");

    while (true)
    {
        var (row, col) = Board.WaitForClick();
        if (row < 0) return;                         // Fenster wurde geschlossen

        string aktuell = Board.GetText(row, col);
        if (aktuell == "")
            Board.SetText(row, col, "X", "Red");     // leer → markieren
        else
            Board.SetText(row, col, "");             // markiert → löschen
    }
}
```

> **Tipp:** `Board.WaitForClick()` gibt `(-1, -1)` zurück, wenn das Fenster geschlossen wird.  
> Überprüfe nach jedem Klick mit `if (row < 0) return;`, um das Programm sauber zu beenden.

---

## Beispiel 3 – Klicks pro Zelle zählen

```csharp
public static void Run()
{
    int zeilen = 3, spalten = 3;
    Board.Init(zeilen, spalten, "Klick-Zähler");

    int[,] zaehler = new int[zeilen, spalten];   // speichert die Klickanzahl pro Zelle

    Console.WriteLine("Klicke auf Zellen. Strg+C zum Beenden.");
    while (true)
    {
        var (row, col) = Board.WaitForClick();
        if (row < 0) return;

        zaehler[row, col]++;
        Board.SetText(row, col, zaehler[row, col].ToString(), "Green");
    }
}
```

---

## Übungsaufgaben

1. **Diagonale** – Schreibe `"X"` auf die Hauptdiagonale (oben links nach unten rechts) eines 5×5-Boards.
2. **Schachbrettmuster** – Fülle das Board so, dass sich `"■"` (schwarz) und `""` (leer) abwechseln wie auf einem Schachbrett.
3. **Suchen & Hervorheben** – Fülle das Board mit zufälligen einstelligen Ziffern (0–9). Warte dann auf einen Klick und hebe alle Zellen hervor, die dieselbe Ziffer enthalten wie die angeklickte Zelle.
4. **Zählerspiel** – Zeige eine Zahl in der Mittelzelle an. Jeder Klick auf eine beliebige Zelle erhöht die Zahl. Nach 10 Klicks wird das Ergebnis auf der Konsole ausgegeben.
