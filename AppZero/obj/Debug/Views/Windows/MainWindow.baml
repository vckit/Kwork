   M S B A M L   `   `   `  ????B  >AppZero, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:AppZeroAppZero  5   	       AppZero.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:AppZero     5   	     ? WindowStartupLocation$  CenterScreen=?5   	   $	??500??5	   	   $	??830??5
   	   $	??450??5   	   $	??830??6    ? Title 5   	   ??  ?? ElementName$ 	mainFrame?? ?? Path$ Content.Title?.??5      ?  ? RowDefinitions 5   
   ?? 5      $??30 ?6   ?? 5      $??* ?6   .+?5   
   ?? $??0??6   
???   ?$??1q?62   ._?5      ? 
???????.+?5      ?? $
??Left=?6   $??10,0,0,0q?5      $??Center=?5      .+?5      ?? -   5        $??
btnSignOut??5      $??
Выйти??5      $	??100??5       ? 	Resources 5       ?? 
TargetType ?? Property ?? Value??%d   '??       ?? 5      " ??6$   .?5   "   ??  ?@??CornerRadius$ 10??6)   64   5      5       5!      5"      5#      5$      ? 5&   
   $??1??6   .+?5'       ? -   5        $??	mainFrame??5'      	  ? NavigationUIVisibility$	 Hidden=?6%   6E   5(      5*      5+      