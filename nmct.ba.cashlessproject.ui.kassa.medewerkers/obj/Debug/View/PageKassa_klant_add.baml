   M S B A M L   `   `   `  ����[  Wnmct.ba.cashlessproject.ui.kassa, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullg8clr-namespace:nmct.ba.cashlessproject.ui.kassa.ViewModel*nmct.ba.cashlessproject.ui.kassa.ViewModel  5      ?    9nmct.ba.cashlessproject.ui.kassa.View.PageKassa_klant_add   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Gvm8clr-namespace:nmct.ba.cashlessproject.ui.kassa.ViewModel     5        � 	Resources  5	      F   @nmct.ba.cashlessproject.ui.kassa.ViewModel.PageKassaVM_klant_add   PageKassaVM_klant_addG   Anmct.ba.cashlessproject.ui.kassa.ViewModel.StringToImageConverter  StringToImageConverter��%:   &	        &	        5
   
   6#     5   
   6$   5       � DataContext 5        5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��2* �6   �� 5      $��4* �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��1* �6   �� 5      $��40 �6   .+�5   
   �� -   5         ��Name$ lblPaginaNaam��5        label_pagina#��[ 6'   $��0��6f   $��2��6v   ��6M   �� *Name+6v   � 5    
   $��0,10,0,0q�6   $��0��6!   $��1��61   $	��Top=�6>   $��3��6V   .+�5"      �� $��Stretch=�6   $
��Left=�67   $
��auto=�6M   ��.  � UseLayoutRounding .  $��0q�6�   .��5$      ��  �� Orientation$ Vertical=�6    64   ��  �� Source# [   �� Path$ Customer�	 �� Mode$	 TwoWay=�.+�5%      �  5&      �� 5'      $��1* �6/   �� 5(      $��6* �6/   .+�5*      ��   	textblock#��[ 6$   $
��Naam��6G   $��2��6S   $
��Left=�6g   $��Center=�6�   6�   �� 5+        ErrorTemplate
 =� ErrorTemplate#
 [ 6x     login_textbox#��[ 6�   $��2��6�   $��1��6�   ��6"   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . 6�   5-      � 5.       5/      �� 50      $��1* �6/   �� 51      $��6* �6/   .+�53      �� #��[ 6$   $��Familienaam��6G   $��2��6Z   $
��Left=�6n   $��Center=�6�   6�   �� 54      #
 [ 6{   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *	SurName+$ PropertyChanged=� . 6�   56      � 57       58      �� 59      $��1* �6/   �� 5:      $��6* �6/   .+�5<      �� #��[ 6$   $��Adres��6G   $��2��6T   $
��Left=�6h   $��Center=�6�   6�   �� 5=      #
 [ 6{   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *	Address+$ PropertyChanged=� . 6�   5?      � 5A       5B      �� 5C      $��1* �6/   �� 5D      $��6* �6/   .+�5F      �� #��[ 6$   $��Registernummer��6G   $��2��6]   $
��Left=�6q   $��Center=�6�   6�   �� 5G      #
 [ 6�   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *RegisterNumber+$ PropertyChanged=� . 6�   5I      � 5K       5L      �� 5M      $��1* �6/   �� 5N      $��6* �6/   .+�5P      �� #��[ 6$   $��
Afbeelding��6G   $��2��6Y   $
��Left=�6m   $��Center=�6�   6�   �� 5Q      $	��200��6n   $
��auto��6}   $
��Fill=�6�    J� 	AllowDrop . ��6    �� *	Picture+ �� 	Converter# [ 6�   5R      5T      5U      5V      �� 5W   
   $��2��6   $��3��6!   $	��200��6|   $��Bottom=�6�   $��Right=�6�   $��Voeg toe��6�   $��40��6�   ��6.   �� # [  $ Insert�6�   �� 5X   
    
 cancel#��[ 6   $��0��61   $��3��6A   $��2��6N   $	��200��6}   $��Bottom=�6�   $
��Left=�6�   $��Annuleer��6�   $��40��6�   ��6b   �� *Cancel+6�   5Z      5[      