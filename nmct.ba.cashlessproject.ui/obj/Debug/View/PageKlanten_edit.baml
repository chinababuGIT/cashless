   M S B A M L   `   `   `  ����U  Qnmct.ba.cashlessproject.ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel$nmct.ba.cashlessproject.ui.ViewModel  5      f bGalaSoft.MvvmLight.Platform, Version=5.0.2.32241, Culture=neutral, PublicKeyToken=5f873c45e98af8a1lMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.PlatformGalaSoft.MvvmLight.Command 5      6    0nmct.ba.cashlessproject.ui.View.PageKlanten_edit   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Avm2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel     5      ]cmdMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5        � 	Resources  5      A   ;nmct.ba.cashlessproject.ui.ViewModel.StringToImageConverter   StringToImageConverter=   7nmct.ba.cashlessproject.ui.ViewModel.PageKlantenVM_edit  PageKlanten_edit��%:   &	        &	        5   
   6$     5   
   6    5       � DataContext 5        5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��2* �6   �� 5      $��4* �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��1* �6   �� 5      $��40 �6   .+�5   
   �� -   5         ��Name$ lblPaginaNaam��5        label_pagina#��[ 6'   $��0��6f   $��2��6v   ��6M   �� *Name+6v   � 5!   
   $��0,10,0,0q�6   $��0��6#   $��1��63   $	��Top=�6@   $��3��6X   .+�5#      �� $��Stretch=�6   $
��Left=�67   $
��auto=�6M   ��.  � UseLayoutRounding .  $��0q�6�   .��5%      ��  �� Orientation$ Vertical=�6    64   ��  �� Source# [  �� Path$ Customer�	 �� Mode$	 TwoWay=�.+�5&      �  5'      �� 5(      $��1* �6/   �� 5)      $��6* �6/   .+�5+      ��   	textblock#��[ 6$   $
��Naam��6G   $��2��6S   $
��Left=�6g   $��Center=�6�   6�   �� 5,        ErrorTemplate
 =� ErrorTemplate#
 [ 6x     login_textbox#��[ 6�   $��2��6�   $��1��6�   ��6"   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . 6�   5.      � 5/       50      �� 51      $��1* �6/   �� 52      $��6* �6/   .+�54      �� #��[ 6$   $��Familienaam��6G   $��2��6Z   $
��Left=�6n   $��Center=�6�   6�   �� 55      #
 [ 6{   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *	SurName+$ PropertyChanged=� . 6�   57      � 58       59      �� 5:      $��1* �6/   �� 5;      $��6* �6/   .+�5=      �� #��[ 6$   $��Adres��6G   $��2��6T   $
��Left=�6h   $��Center=�6�   6�   �� 5>      #
 [ 6{   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *	Address+$ PropertyChanged=� . 6�   5@      � 5C       5D      �� 5E      $��1* �6/   �� 5F      $��6* �6/   .+�5H      �� #��[ 6$   $��Balans��6G   $��2��6U   $
��Left=�6i   $��Center=�6�   6�   �� 5I      #
 [ 6{   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *	Balance+$ PropertyChanged=� . 6�   5K      � 5M       5N      �� 5O      $��1* �6/   �� 5P      $��6* �6/   .+�5R      �� #��[ 6$   $��Registernummer��6G   $��2��6]   $
��Left=�6q   $��Center=�6�   6�   �� 5S      #
 [ 6�   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *RegisterNumber+$ PropertyChanged=� . 6�   5U      � 5W       5X      �� 5Y      $��1* �6/   �� 5Z      $��6* �6/   .+�5\      �� #��[ 6$   $��
Afbeelding��6G   $��2��6Y   $
��Left=�6m   $��Center=�6�   6�   �� 5]      $��2��6!   $��1��65   ��6E   �� # [ $ AddImageCommand���5^      �� 5_   "   .R�5a   &   �� $	��200��6z   $
��auto��6�   $
��Fill=�6�    J� 	AllowDrop . ��6,   �� *	Picture+ �� 	Converter# [  .  (System.Windows.Interactivity.Interaction   Triggers 5b   *   /  )System.Windows.Interactivity.EventTrigger  5c   .      	EventName$
 Drop��6=   .  (System.Windows.Interactivity.TriggerBase   Actions. 5d   2   /  )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand .    Command 6E   �� # [ $ DropImageCommand�6�   5e   /   5g   '   5i   #   5k      5l      5p      5q      5r      �� 5s   
   $��2��6   $��3��6!   $	��100��6w   $��Bottom=�6�   $��Right=�6�   $��Pas aan��6�   $��40��6�   ��6.   �� # [ $ Update�6�   �� 5t   
   $��2��6   $��3��6!   $	��100��6w   $��Bottom=�6�   $��Right=�6�   $��	0,0,110,0q�6�   $��	Verwijder��6�   $��40��6�   ��6.   �� # [ $ Delete�6�   �� 5v   
    
 cancel#��[ 6   $��0��61   $��3��6A   $��2��6N   $	��200��6}   $��Bottom=�6�   $
��Left=�6�   $��Annuleer��6�   $��40��6�   ��6b   �� *Cancel+6�   5x      5y      