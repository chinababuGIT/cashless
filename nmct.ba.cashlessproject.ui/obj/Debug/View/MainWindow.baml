   M S B A M L   `   `   `  ����U  Qnmct.ba.cashlessproject.ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel$nmct.ba.cashlessproject.ui.ViewModel  5   	   Q-clr-namespace:nmct.ba.cashlessproject.ui.Viewnmct.ba.cashlessproject.ui.View  5   	   0    *nmct.ba.cashlessproject.ui.View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Avm2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel     5   	   ;v-clr-namespace:nmct.ba.cashlessproject.ui.View     5   	   \cmdLclrnamespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	     � Title$  Cashless��5   	   $	��600��6   $	��800��6'    � DataContext 5	      8   2nmct.ba.cashlessproject.ui.ViewModel.ApplicationVM  5
   
    � 	Resources 5      A   ;nmct.ba.cashlessproject.ui.ViewModel.StringToImageConverter   StringToImageConverter>   8nmct.ba.cashlessproject.ui.ViewModel.GridLengthConverter  gridLenghtConverter=   7nmct.ba.cashlessproject.ui.ViewModel.ThicknessConverter  ThicknessConverter �� DataType7   1nmct.ba.cashlessproject.ui.ViewModel.PageKassasVM0   *nmct.ba.cashlessproject.ui.View.PageKassas8   2nmct.ba.cashlessproject.ui.ViewModel.PageKlantenVM1   +nmct.ba.cashlessproject.ui.View.PageKlanten<	   6nmct.ba.cashlessproject.ui.ViewModel.PageKlantenVM_add5
   /nmct.ba.cashlessproject.ui.View.PageKlanten_add=   7nmct.ba.cashlessproject.ui.ViewModel.PageKlantenVM_edit6   0nmct.ba.cashlessproject.ui.View.PageKlanten_edit<   6nmct.ba.cashlessproject.ui.ViewModel.PageMedewerkersVM5   /nmct.ba.cashlessproject.ui.View.PageMedewerkers@   :nmct.ba.cashlessproject.ui.ViewModel.PageMedewerkersVM_add9   3nmct.ba.cashlessproject.ui.View.PageMedewerkers_addA   ;nmct.ba.cashlessproject.ui.ViewModel.PageMedewerkersVM_edit:   4nmct.ba.cashlessproject.ui.View.PageMedewerkers_edit:   4nmct.ba.cashlessproject.ui.ViewModel.PageProductenVM3   -nmct.ba.cashlessproject.ui.View.PageProducten?  9nmct.ba.cashlessproject.ui.ViewModel.PageProductenVM_edit8   2nmct.ba.cashlessproject.ui.View.PageProducten_edit>   8nmct.ba.cashlessproject.ui.ViewModel.PageProductenVM_add7   1nmct.ba.cashlessproject.ui.View.PageProducten_add8   2nmct.ba.cashlessproject.ui.ViewModel.PageProfielVM1   +nmct.ba.cashlessproject.ui.View.PageProfiel6   0nmct.ba.cashlessproject.ui.ViewModel.PageStartVM/   )nmct.ba.cashlessproject.ui.View.PageStart=   7nmct.ba.cashlessproject.ui.ViewModel.PageStatistiekenVM6   0nmct.ba.cashlessproject.ui.View.PageStatistieken6   0nmct.ba.cashlessproject.ui.ViewModel.PageLoginVM/    )nmct.ba.cashlessproject.ui.View.PageLogin��%q  &	        &	      &	 &     (�� 9     5      *, +)(�� k     5      *, +)(�� �     5      *,	 +)(�� �     5      *, +)(��     5      *, +)(�� 3    5!      *, +)(�� e    5$      *, +)(�� �    5'      *, +)(�� �    5*      *, +)(�� �    5-      *, +)(�� -    50      *, +)(�� _    53      *, +)(�� �    56      *, +)(�� �    59      *, +)  5   
   6$     5   
   6!     5   
   6    �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   " 	 6   .R�5      
  5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5       �� 5!   
   "  6   .R�5"        5#      �� 5$   
   "  6   .R�5%        5&      �� 5'   
   "  6   .R�5(        5)      �� 5*   
   "  6   .R�5+        5,      �� 5-   
   "  6   .R�5.        5/      �� 50   
   "  6   .R�51        52      �� 53   
   "  6   .R�54        55      �� 56   
   "  6   .R�57        58      �� 59   
   "  6   .R�5:         5;      5=      .��5?      �   
background#��[ 6    � ColumnDefinitions 5@      �� 5B      $	��250��5D   |   ��5B      �� *NavigationWidth+ �� Mode$ TwoWay=� �� 	Converter# [ ��5D   T   �� *NavigationMinWidth+6|   �� 5F      $��3* �6   .+�5I   
   �� $
��Left=�6   $
��auto=�6-   ��.  � UseLayoutRounding .  $��0q�6�   .��5J      �� $
��Auto=�6     Blue#��[ 6I    �� Orientation$ Vertical=�6m   .+�5L      ��   logo#��[ 6   $��0��67   .��5M      �� $B��</nmct.ba.cashlessproject.ui;component/Themes/images/logo.png��6   5N      �� 5R      ��6   �� *Pages+��5S      �� 5T      .R�5U      ��   menu_button#��[ 6�   ��6%   �� *DataContext.ChangePageCommand+	 �� RelativeSource	 �� 
 �� AncestorType"
 ���6�   �� ��6�   �� *
Selected+# [ .��5V   "   � $��0,6,0,0q�6'    5W   &   �� 5X   *   $��1* �6;   �� 5Y   *   $��3* �6;   .+�5\   &   �� $��15,0,0,0q�6,   $��0��6>   $	��Top=�6N   $��30��6f   ��6r   �� *Image+�� 5]   &     
menu_label#��[ 6,   $��35��6P   $��1��6\   ��6l   �� *Name+5^   #   5_      5a      5e      �� 5h      $��	25,10,0,0q�6   #��[ 6,   $��Wachtwoord wijzigen��6{   $��1,1,10,1q�6�   $��0��6�   $��Center=�6�   ��6Q   �� *ChangePasswordCommand+6�   �� 5j      $��25,0,0,0q�6   #��[ 6+   $��Afmelden��6r   $��1,1,10,1q�6�   $��0��6�   $��Center=�6�   ��6P   �� *LogOffCommand+6�   5n      5o      � 5q   
   $��1��6   .+�5r      �� -   5         ��Name$
 main��5r      ��6+   �� *CurrentPage+5s      5w      5x      