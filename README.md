### 1. Modèle `Album`

Le modèle représente les données d'un album.

```csharp
namespace MyApp.Model
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
    }
}
```
- **Id** : Identifiant de l'album.
- **Title** : Titre de l'album.
- **Cover** : URL de la couverture de l'album.

### 2. Service `AlbumService`

Le service récupère les données des albums à partir d'une API.

```csharp
using MyApp.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace MyApp.Service
{
    public class AlbumService
    {
        private const int MAX = 500;
        private const int MIN = 1;

        public async Task<IEnumerable<Album>> GetAllAsync()
        {
            var random = new Random();
            var uri = "https://jsonplaceholder.typicode.com/albums";
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(uri);
            var albums = JsonConvert.DeserializeObject<List<Album>>(response);

            foreach (var album in albums)
            {
                album.Cover = $"https://picsum.photos/200?{random.Next(MIN, MAX)}";
            }
            return albums;
        }
    }
}
```
- **GetAllAsync()** : Récupère les albums depuis l'API, ajoute une URL de couverture aléatoire, et retourne la liste des albums.

### 3. Vue `MainPage.xaml`

Définit l'interface utilisateur pour afficher les albums.

```xml
<Page x:Class="MyApp.MainPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:local="clr-namespace:MyApp"
      mc:Ignorable="d" 
      d:DesignHeight="450" d:DesignWidth="800"
      Title="MainPage"
      Background="White">

    <Grid>
        <StackPanel Orientation="Vertical">
            <ListView x:Name="listView" ItemsSource="{Binding Albums}">
                <ListView.ItemTemplate>
                    <DataTemplate>
                        <StackPanel Orientation="Vertical">
                            <Image Source="{Binding Cover}" 
                                   Width="252"
                                   Height="252"
                                   HorizontalAlignment="Center"
                                   VerticalAlignment="Center"/>
                            <TextBlock Text="{Binding Title}"/>
                        </StackPanel>
                    </DataTemplate>
                </ListView.ItemTemplate>
            </ListView>
        </StackPanel>
    </Grid>
</Page>
```
- **ListView** : Affiche une liste d'albums avec des images et des titres.

### 4. Code-behind `MainPage.xaml.cs`

Gère la logique pour charger et afficher les albums.

```csharp
using MyApp.Model;
using MyApp.Service;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MyApp
{
    public partial class MainPage : Page
    {
        public ObservableCollection<Album> Albums { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Albums = new ObservableCollection<Album>();
            DataContext = this;

            LoadAlbumsAsync();
        }

        private async void LoadAlbumsAsync()
        {
            try
            {
                var service = new AlbumService();
                var result = await service.GetAllAsync();

                foreach (var item in result)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Albums.Add(item);
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading albums: {ex.Message}");
            }
        }
    }
}
```
- **Albums** : Collection observable pour stocker et afficher les albums.
- **LoadAlbumsAsync()** : Charge les albums de manière asynchrone et les ajoute à la collection.

### 5. Fenêtre principale `MainWindow.xaml`

Héberge la `MainPage`.

```xml
<Window x:Class="MyApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="450" Width="800">
    <Frame Source="MainPage.xaml"/>
</Window>
```
- **Frame** : Navigue vers `MainPage.xaml` et affiche son contenu.

### 6. Code-behind `MainWindow.xaml.cs`

Initialise la fenêtre principale.

```csharp
using System.Windows;

namespace MyApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
```

### 7. Configuration de l'application `App.xaml`

Définit le point de départ de l'application.

```xml
<Application x:Class="MyApp.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:local="clr-namespace:MyApp"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
    </Application.Resources>
</Application>
```
- **StartupUri** : Spécifie que l'application démarre avec `MainWindow.xaml`.

### Résumé global :
- **Album** : Modèle de données pour un album.
- **AlbumService** : Service pour récupérer les albums depuis une API et ajouter une couverture.
- **MainPage** : Vue et code-behind pour afficher les albums dans une liste.
- **MainWindow** : Fenêtre principale qui héberge `MainPage`.
- **App.xaml** : Configuration de l'application pour démarrer avec `MainWindow`.

Ce code constitue une application WPF de base pour afficher des albums récupérés depuis une API dans une interface utilisateur.


### Explication détaillée de `MainPage.xaml` et du principe de la liaison de données (binding)

#### 1. `MainPage.xaml`

`MainPage.xaml` est un fichier XAML (Extensible Application Markup Language) qui définit l'interface utilisateur de la page principale de l'application. Voici le code détaillé :

```xml
<Page x:Class="MyApp.MainPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:local="clr-namespace:MyApp"
      mc:Ignorable="d" 
      d:DesignHeight="450" d:DesignWidth="800"
      Title="MainPage"
      Background="White">

    <Grid>
        <StackPanel Orientation="Vertical">
            <ListView x:Name="listView" ItemsSource="{Binding Albums}">
                <ListView.ItemTemplate>
                    <DataTemplate>
                        <StackPanel Orientation="Vertical">
                            <Image Source="{Binding Cover}" 
                                   Width="252"
                                   Height="252"
                                   HorizontalAlignment="Center"
                                   VerticalAlignment="Center"/>
                            <TextBlock Text="{Binding Title}"/>
                        </StackPanel>
                    </DataTemplate>
                </ListView.ItemTemplate>
            </ListView>
        </StackPanel>
    </Grid>
</Page>
```

##### Explication des éléments XAML

1. **Déclaration de la page**:
   ```xml
   <Page x:Class="MyApp.MainPage"
         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
         xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
         xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
         xmlns:local="clr-namespace:MyApp"
         mc:Ignorable="d" 
         d:DesignHeight="450" d:DesignWidth="800"
         Title="MainPage"
         Background="White">
   ```
   - `x:Class="MyApp.MainPage"` : Indique que cette page est associée à la classe `MainPage` dans l'espace de noms `MyApp`.
   - `xmlns` : Définit les espaces de noms XML utilisés dans le document.
   - `mc:Ignorable="d"` : Indique que les attributs de conception (`d:`) peuvent être ignorés à l'exécution.
   - `d:DesignHeight` et `d:DesignWidth` : Dimensions de conception de la page.

2. **Grille principale**:
   ```xml
   <Grid>
       <StackPanel Orientation="Vertical">
           <ListView x:Name="listView" ItemsSource="{Binding Albums}">
               <ListView.ItemTemplate>
                   <DataTemplate>
                       <StackPanel Orientation="Vertical">
                           <Image Source="{Binding Cover}" 
                                  Width="252"
                                  Height="252"
                                  HorizontalAlignment="Center"
                                  VerticalAlignment="Center"/>
                           <TextBlock Text="{Binding Title}"/>
                       </StackPanel>
                   </DataTemplate>
               </ListView.ItemTemplate>
           </ListView>
       </StackPanel>
   </Grid>
   ```
   - `Grid` : Conteneur de disposition principal pour organiser les éléments enfants.
   - `StackPanel` : Organise les enfants verticalement (`Orientation="Vertical"`).
   - `ListView` : Affiche une liste d'éléments. 
     - `ItemsSource="{Binding Albums}"` : Spécifie que la source des éléments de la liste est la propriété `Albums` du contexte de données.
     - `ListView.ItemTemplate` : Définit la structure visuelle pour chaque élément de la liste.
       - `DataTemplate` : Contient le modèle de données pour chaque élément.
         - `StackPanel` : Conteneur pour les éléments de chaque item.
           - `Image` : Affiche l'image de couverture de l'album. La source de l'image est liée à la propriété `Cover`.
           - `TextBlock` : Affiche le titre de l'album. Le texte est lié à la propriété `Title`.

#### 2. Principe du Binding (Liaison de données)

Le binding en WPF (Windows Presentation Foundation) est une technique permettant de lier les propriétés des éléments de l'interface utilisateur aux propriétés d'objets de données. Le binding facilite la synchronisation entre l'interface utilisateur et les données sous-jacentes.

##### Comment le binding fonctionne

1. **DataContext**:
   Le `DataContext` est l'objet de données auquel les éléments de l'interface utilisateur sont liés. Dans `MainPage.xaml.cs`, le `DataContext` est défini comme `this`, ce qui signifie que les éléments de l'interface utilisateur sont liés aux propriétés de l'instance `MainPage`.

   ```csharp
   public MainPage()
   {
       InitializeComponent();
       Albums = new ObservableCollection<Album>();
       DataContext = this;
       LoadAlbumsAsync();
   }
   ```

2. **ItemsSource Binding**:
   L'attribut `ItemsSource="{Binding Albums}"` lie la source des éléments de la `ListView` à la propriété `Albums` de `MainPage`.

3. **Propriétés liées dans les `DataTemplate`**:
   - `Image Source="{Binding Cover}"` : Lie la propriété `Source` de l'image à la propriété `Cover` de l'élément de données.
   - `TextBlock Text="{Binding Title}"` : Lie la propriété `Text` du `TextBlock` à la propriété `Title` de l'élément de données.

##### Avantages du Binding

- **Séparation des préoccupations** : Sépare la logique de présentation des données.
- **Synchronisation automatique** : Met à jour automatiquement l'interface utilisateur lorsque les données changent.
- **Facilité de test** : Permet de tester la logique de l'application indépendamment de l'interface utilisateur.

En résumé, `MainPage.xaml` définit une interface utilisateur pour afficher une liste d'albums en utilisant le binding pour lier les propriétés de l'interface utilisateur aux propriétés des objets de données, ce qui permet une mise à jour dynamique et synchrone de l'interface utilisateur en fonction des changements de données.