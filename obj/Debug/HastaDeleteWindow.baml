   M S B A M L   `   `   `  ????N  JPoliklinikOtomasyon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:PoliklinikOtomasyonPoliklinikOtomasyon  5   	   +    %PoliklinikOtomasyon.HastaDeleteWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:PoliklinikOtomasyon     5   	     ? Title$  Hasta Kayıt Sil??5   	   $??645.49??6!   $??1085.302??61    ? 
ResizeMode$ NoResize=?6B   .??5	      ? $??0,0,-4.5,2.5q?6   $
??Left=?6!   $	??645??6<   .+?5
   
   ?? $??Hasta Kayıt Sil??6   $
??Left=?6*   $??10,8,0,0q?6E   $	??Top=?6W   $
??Bold"?6o   $??Segoe UI Black)?6?   ???	#FFEBA83F$??63??6?   $	??324??6?   $??36(?6?   ??6?   Z? 6?   ?? 5   
   $-??'Silmek İstediğiniz Hastayı seçiniz.??6   $
??Left=?6>   $??	10,53,0,0q?6Y   $	??Top=?6l   $??16(?6?   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ hastaDataGrid??5      $
??Left=?6*   $	??437??6E   $??10,101,-402,0q?6R   $	??Top=?6i   $
??1037??6?   4  .System.Windows.Automation.AutomationProperties   Name$ hastaDataGrid??6?   ???	#FF646464
??????????	#FF262626   SelectionMode$ Single=?6,     AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6_  $
??Bold"?6o     HorizontalGridLinesBrush ?	#FF3C3C3C   VerticalGridLinesBrush ?	#FF343434	 J? Opacity$		 0.8Q?6?  
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Id??6<      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ?? *Id+6H     5      $ Ad Soyad??6C    .  6%   ?? *	AdiSoyadi+6U     5      $ Kimlik Numarası??6B    .  6%   ?? *
KimlikNo+6[     5      $ Telefon Numarası??6C    .  6%   ?? *	TelefonNo+6]     5      $ Doğum Tarihi??6E    .  6%   ?? *DogumTarihi+6[     5      $ Cinsiyet??6C    .  6%   ?? *	Cinsiyeti+6U     5      $ Randevu Tarihi??6G    .  6%   ?? *RandevuTarihi+6_     5      $ Kayıt Tipi??6C    .  6%   ?? *	KayitTipi+6W     5      $ Doktor Adı??6C    .  6%   ?? *	DoktorAdi+6W   5      ?? 5   
   -   5         ??Name$ deleteButton??5      $
??SİL??6'   $
??Left=?65   $??680,555,-402,0q?6P   $	??Top=?6h   $	??367??6?   $??37??6?   $??Segoe UI Black)?6?   $
??Bold"?6?   ???	#FFEBA83F
???????$ deleteButton??6?   {?.  ??64  Z? 6K  ?? 5   
   -   5         ??Name$ hastaTcKimlikNoTextBox??5      $
??Left=?62   $??23??6M   $??680,65,-402,0q?6Y    ?? TextWrapping$
 Wrap=?6p   $	??Top=?6?   $	??367??6?   ???	#FF313131$ hastaTcKimlikNoTextBox??6?   6?   ?? 5   
   $??Hasta Kimlik Numarası??6   $
??Left=?60   $??676,42,-160,0q?6K   $	??Top=?6b   5      5      