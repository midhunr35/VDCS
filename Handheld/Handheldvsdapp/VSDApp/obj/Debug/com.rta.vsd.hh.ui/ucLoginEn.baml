   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      Z VWPFToolkit.Extended, Version=1.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4dEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.ExtendedMicrosoft.Windows.Controls 5	      (    "VSDApp.com.rta.vsd.hh.ui.ucLoginEn   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      BwpfNameSpace)http://schemas.xceed.com/wpf/xaml/toolkit     5      6
propertiesclr-namespace:VSDApp.Properties     5      \
extToolkitEclr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit.Extended     5	      -   5        $
��Auto��5
      $
��Auto��6   $��Stretch=�6(   $��Stretch=�6F   $��Stretch=�5      $��Stretch=�63     � 	Resources  5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      .  (Microsoft.Windows.Controls.BusyIndicator  -   5          Name$ busyIndicator��5         IsBusy .  .��5   
   � -   5         �Name$ GridMain��5      $y�Visible=�6!   ���#F1F1F1 � RowDefinitions 5      �� 5      $��* �6    �� 5      $
��0.8* �6     � ColumnDefinitions 5      �� 5      $��* �6#   .+�5       �� -   5         ��Name$ stckPanelUpperMain��5       	 �� Orientation$	 
Horizontal=�65   $��0��6N   $��0��6[   $��Center=�6k   .+�5!      �� -   5        $ stackPanelUpperLeft��5!      $��Center=�6:   .+�5"      �  5#      �� 5$      $��10 �6,   �� 5%      $
��Auto �6,   �� 5&      $��15 �6,   �� 5'      $
��Auto �6,   �� 5(      $��15 �6,   �� 5)      $
��Auto �6,   �� 5*      $��15 �6,   �� 5+      $
��Auto �6,   �� 5,      $��5 �6,    50      �� 51      $��35 �6/   �� 52      $��* �6/   �� 53      $��35 �6/   .+�55      �� -   5        
 ��Name$
 	lblRtaApp��55       !   VSDApp.Properties.Resources   RTAVSDApplication#��Z 63   $��-5,0,0,0q�6o   $��1��6�   $��1��6�   $��Center=�6�      LabelStyleLoginLarge#��[  6�   �� 56      -   5        $
 lblRtaSelectLan��56          SelectLanguage#��Z 69   $��-5,0,0,0q�6r   $��3��6�   $��1��6�   $
��Left=�6�     LabelStyleLoginSmall#��[ 6�   �� 57      -   5         ��Name$ cmbLanguageSelection��57   #   $��5��6A   $��1��6N   $	��345��6^   $��45��6j   $
��Left=�6v     cmbLoginSelctionLoginPage#��[ 6�   
�������59      5;      5<      �� 5=      -	   5        $ stackPanelUpperRight��5=      .+�5>      �  5?      �� 5@      $��10 �6,   �� 5A      $
��Auto �6,   �� 5B      $��15 �6,   �� 5C      $
��Auto �6,   �� 5D      $��15 �6,   �� 5E      $
��Auto �6,   �� 5F      $��15 �6,   �� 5G      $
��Auto �6,   �� 5H      $��15 �6,   �� 5I      $
��Auto �6,   �� 5J      $��15 �6,    5M      �� 5N      $��35 �6/   �� 5O      $��* �6/   �� 5P      $��35 �6/   .+�5R      �� -
   5        $	
 lbl��5R       #   Fillintherequiredinformation#��Z 6-   $
��Left=�6t   $��-5,5,0,0q�6�   #��[ 6�   $��1��6�   $��1��6�   � 5S      -   5         �Name$ txtBoxUserNamePopup��5S       $	��Top=�6=   ��.   � PlacementTarget 6N   ��  �� ElementName$ txtBoxUserName��.��5T      �� -   5         ��Name$ PopupTextBlock��5T   (   $	��Top=�6@   ���#FF3334
�������$��17(�6�   .,Hint: Username and Password is Key Sensitive6�   5U      �� 5V      -   5         ��Name$ txtBoxUserName��5V   "     TextBoxLogin#��[ 6<   $��3��6b   $��1��6o   $��Center=�6�   
�������$
��Left=�6�   6~  G� 5X      -   5         G�Name$ txtpswd��5X   &    G� PasswordChar$ *��67   $��5��6I   
�������$��1��6h     TextBoxPassword#��[ 6x   $
��Left=�6�   $��Center=�6�   6P  �� 5\      -   5        $ txtBlockForgotName��5\   $   $y�	Collapsed=�6@   $��	Underline{�6W   $��7��6u   $��1��6�   $��Segoe UI)�6�   $��Medium"�6�   $��17(�6�   $
��Left=�6�   �� 5]      -   5         ��Name$ hyperlinkForgotName��5]      .�6`   .�5^      �� -   5           lblForgotUsername#��Z 5^      $y�	Collapsed=�6W   6n   5_      5a      �� 5c      -   5         ��Name$ btnLogin��5c   !     ButtonLoginPage#��[ 64      BtnLogin#��Z 6^   $��9��6�   $��1��6�   $
��Left=�6�   6�   5e      5g      5h      �� 5i      -   5        $ stckPanelUpperMain2��5i      $��1��67   .+�5j      �� $#��..\Images\login_landscape.jpg��6   5k      � 5l      -   5        $ maingrid��5l      $y�	Collapsed=�6%   ��5m      �� 5n       �� ImageSource$" ..\Images\background_New.png��6!    5p      �� 5q      $��* �6$   �� 5r      $
��Auto �6$   �� 5s      $��15 �6$   �� 5t      $
��Auto �6$   �� 5u      $��40 �6$   �� 5v      $
��Auto �6$   �� 5w      $��15 �6$   �� 5x      $
��Auto �6$   �� 5y      $��20 �6$   �� 5z      $��15 �6$   �� 5{      $
��Auto �6$   �� 5|      $��* �6$   �� 5}      $��* �6$    5      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   �� 5�      $��10 �6'   �� 5�      $
��Auto �6'   �� 5�      $��* �6'   .+�5�      �� -   5        $ 
 lblMessageBoxNotAuthorized��5�      $
��Left=�6<   $��1��6W   $��1��6d   !   LblMessageBoxNotAuthorized#��Z 6t   $��15(�6�   $y�	Collapsed=�6�   �� 5�      -   5        $
 lblLoggingIn��5�      $
��Left=�6.   $��1��6I   $��1��6V      lblLoggingIn#��Z 6f   $��15(�6�   $y�	Collapsed=�6�   �� 5�      #��Z 6   $
��Left=�6R   $
��Left=�6m   $��1��6�   $��3��6�   $��Medium"�6�   $��Segoe UI)�6�   $��30(�6�   �� 5�      -   5        $ txtBlockLanguageEng��5�      $��	Underline{�69   $��3��6V   $��3��6c   $��Segoe UI)�6s   $��Medium"�6�   $��25(�6�   $
��Left=�6�   �� 5�      -   5        $ hyperlinkEng��5�      .�6N   	English6X   5�      �� 5�      -   5        $ txtBlockLanguageArb��5�      $��	Underline{�69   
����  �$��3��6h   $��3��6u   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   $��Right=�6�   �� 5�      -   5        $ hyperlinkArb��5�      .�6O   العربية6Y   5�      �� 5�      -   5        $
 lblUsername��5�      $��Medium"�6-   $��Segoe UI)�6B   $��25(�6Y   $
��Left=�6h   $��5��6�   $��1��6�      UserName#��Z 6�   �� 5�      $��Arabic Transparent)�6   $y�	Collapsed=�68   $��25(�6O   $
��Left=�6^   $��5��6y   $��0��6�   #��Z 6�   �� 5�      $
��Left=�6   $��Medium"�64   $��7��6H   $��1��6U      Password#��Z 6e   $��25(�6�   $��Segoe UI)�6�   �� 5�      $y�	Collapsed=�6   $
��Left=�60   $��7��6K   $��0��6X   #��Z 6h   $��25(�6�   $��Arabic Transparent)�6�   �� 5�      -   5         ��Name$ imagebuttonetest2��5�      $'��!..\Images\Buttons\Large\Login.png��63   $	��350��6^   $��10��6j   $��3��6y   $��Stretch=�6�   $��Stretch=�6�   6  5�      5�      5�      5�      