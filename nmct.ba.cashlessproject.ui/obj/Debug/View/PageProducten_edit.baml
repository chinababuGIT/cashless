   M S B A M L   `   `   `  ����U  Qnmct.ba.cashlessproject.ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel$nmct.ba.cashlessproject.ui.ViewModel  5      f bGalaSoft.MvvmLight.Platform, Version=5.0.2.32241, Culture=neutral, PublicKeyToken=5f873c45e98af8a1lMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.PlatformGalaSoft.MvvmLight.Command 5      8    2nmct.ba.cashlessproject.ui.View.PageProducten_edit   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Avm2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel     5      ]cmdMclr-namespace:GalaSoft.MvvmLight.Command;assembly=GalaSoft.MvvmLight.Platform     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5        � 	Resources  5      A   ;nmct.ba.cashlessproject.ui.ViewModel.StringToImageConverter   StringToImageConverter?  9nmct.ba.cashlessproject.ui.ViewModel.PageProductenVM_edit  PageProducten_edit��%:   &	        &	        5   
   6$     5   
   6"   5       � DataContext 5        5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��2* �6   �� 5      $��4* �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��1* �6   �� 5      $��40 �6   .+�5   
   �� -   5         ��Name$ lblPaginaNaam��5        label_pagina#��[ 6'   $��0��6f   $��2��6v   ��6M   �� *Name+6v   � 5!   
   $��0,10,0,0q�6   $��0��6!   $��1��61   $	��Top=�6>   $��3��6V   .+�5#      �� $��Stretch=�6   $
��Left=�67   $
��auto=�6M   ��.  � UseLayoutRounding .  $��0q�6�   .��5%      ��  �� Orientation$ Vertical=�6    64   ��  �� Source# [  �� Path$ Product�	 �� Mode$	 TwoWay=�.+�5&      �  5'      �� 5(      $��1* �6/   �� 5)      $��6* �6/   .+�5+      ��   	textblock#��[ 6$   $
��Naam��6G   $��2��6S   $
��Left=�6g   $��Center=�6�   6�   �� 5,        ErrorTemplate
 =� ErrorTemplate#
 [ 6x     login_textbox#��[ 6�   $��2��6�   $��1��6�   ��6"   �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . 6�   5.      � 5/       50      �� 51      $��1* �6/   �� 52      $��6* �6/   .+�54      �� #��[ 6$   $��Beschrijving��6G   $��2��6[   $
��Left=�6o   $��Center=�6�   6�   �� 55      #
 [ 6   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *Description+$ PropertyChanged=� . 6�   57      � 59       5:      �� 5;      $��1* �6/   �� 5<      $��6* �6/   .+�5>      �� #��[ 6$   $��Prijs��6G   $��2��6T   $
��Left=�6h   $��Center=�6�   6�   �� 5?      #
 [ 6s   #��[ 6�   $��2��6�   $��1��6�   ��6"   �� *Price+$ 	LostFocus=� . 6�   5A      � 5C       5D      �� 5E      $��1* �6/   �� 5F      $��6* �6/   .+�5H      �� #��[ 6$   $��
Afbeelding��6G   $��2��6Y   $
��Left=�6m   $��Center=�6�   6�   �� 5I      $��2��6!   $��1��65   ��6E   �� # [ $ AddImageCommand���5J      �� 5K   "   .R�5M   &   �� $	��200��6z   $
��auto��6�   $
��Fill=�6�    J� 	AllowDrop . ��6,   �� *	Picture+ �� 	Converter# [  .  (System.Windows.Interactivity.Interaction   Triggers 5N   *   /  )System.Windows.Interactivity.EventTrigger  5O   .      	EventName$
 Drop��6=   .  (System.Windows.Interactivity.TriggerBase   Actions. 5P   2   /  )GalaSoft.MvvmLight.Command.EventToCommand     PassEventArgsToCommand .    Command 6E   �� # [ $ DropImageCommand�6�   5Q   /   5S   '   5U   #   5W      5X      5Z      5[      5\      �� 5]   
   $��2��6   $��3��6!   $	��100��6y   $��Bottom=�6�   $��Right=�6�   $��Pas aan��6�   $��40��6�   ��6.   �� # [ $ Update�6�   �� 5^   
   $��2��6   $��3��6!   $	��100��6y   $��Bottom=�6�   $��Right=�6�   $��	0,0,110,0q�6�   $��	Verwijder��6�   $��40��6�   ��6.   �� # [ $ Delete�6�   �� 5`   
    
 cancel#��[ 6   $��0��61   $��3��6A   $��2��6N   $	��200��6}   $��Bottom=�6�   $
��Left=�6�   $��Annuleer��6�   $��40��6�   ��6b   �� *Cancel+6�   5b      5c      