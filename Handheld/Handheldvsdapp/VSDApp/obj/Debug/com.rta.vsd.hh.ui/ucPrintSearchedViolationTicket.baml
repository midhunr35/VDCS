   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      =    7VSDApp.com.rta.vsd.hh.ui.ucPrintSearchedViolationTicket   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      -   5        $
��Auto��5      $
��Auto��6   $��Stretch=�6)   $��Stretch=�6G   
�������$��Stretch=�5      $��Stretch=�6*     � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��1 �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��5 �6   �� 5       $
��Auto �6   �� 5!      $��5 �6   �� 5"      $
��Auto �6   �� 5#      $��5 �6   �� 5$      $
��Auto �6   �� 5%      $��5 �6   �� 5&      $
��Auto �6   �� 5'      $��5 �6   �� 5(      $
��Auto �6   �� 5)      $��5 �6   �� 5*      $
��Auto �6   �� 5+      $��* �6   �� 5,      $��* �6    � ColumnDefinitions 5.   
   �� 5/      $��* �6   �� 50      $
��Auto �6   �� 51      $��10 �6   �� 52      $
��Auto �6   �� 53      $��* �6   .+�55   
   �� -   5         ��Name$ 
lblViolSum��55      !   VSDApp.Properties.Resources   ViolationSummary#��Z 6$   $��Segoe UI)�6_   $��Medium"�6u   $��30(�6�   $��2��6�   $��3��6�   $��1��6�   $��Stretch=�6�   $��Stretch=�6�   �� 56   
   -   5        $ lblOvrAllSev��56         OverAllViolationSeverity#��Z 6&   $��4��6i   $��1��6v   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 57   
   -   5         ��Name$ txtboxViolationSeverity��57      $��4��63   $��3��6@   $	��350��6P   $��40��6\   $��20(�6h   $��Segoe UI)�6v   $��Medium"�6�   $��Stretch=�6�   $��Bottom=�6�   	 �� 
IsReadOnly	 . 6�   �� 58   
   -   5        $ 	lblVioSum��58      
   ViolationID#��Z
 6#   $��6��6Y   $��1��6f   $��Segoe UI)�6v   $��Medium"�6�   $��25(�6�   �� 59   
   -   5        $ txtViolationID��59      $��6��6*   $��3��67   $	��350��6G   $��40��6S   $��20(�6_   $��Segoe UI)�6m   $��Medium"�6�   $��Stretch=�6�   $��Bottom=�6�   	 . 6�   �� 5:   
   -   5        $ 
lblDueDate��5:         DueDate#��Z 6$   $��8��6V   $��1��6c   $��Segoe UI)�6s   $��Medium"�6�   $��25(�6�   �� 5;   
   -   5        $ txtboxDueDate��5;      $��8��6)   $��3��66   $	��350��6F   $��40��6R   $��20(�6^   $��Segoe UI)�6l   $��Medium"�6�   $��Stretch=�6�   $��Bottom=�6�   	 . 6�   �� 5<   
   -	   5        $ lblTechnicalFines��5<         TechnicalFines#��Z 6+   $��10��6d   $��1��6r   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   &   System.Windows.Controls.DataGrid  5=   
   -
   5          Name$ grdAddedFines��5=      $	��350��6*   $��70��66   $��10��6B   $��3��6P      AutoGenerateColumns .  $
��Left=�5>      $��Segoe UI)�6:   $��Medium"�6P   $��Stretch=�6d   $��15(�6�      CanUserDeleteRows .    CanUserAddRows .  $��Stretch=�6�    �� SelectedItem 5=   a   �� *SelectedItem+ �� Mode$ TwoWay=���6�   �� *
AddedFines+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5@      0  *System.Windows.Controls.DataGridTextColumn  5A      -   5        lblFineName��   Fine,  &System.Windows.Controls.DataGridColumn   Header# Z 5A   :      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6{      FontSize$ 15Q�6�      
FontFamily$ Segoe UI)�6�      
FontWeight$ Medium"�6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6�   �� *
FineName+6�     5B      -   5        lblFineAmmount��   Ammount# Z 5B   =    . $ * 6�   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *FineAmmount+6�   5E      �� 5F   
   -   5        $ lblTelematicsFines��5F         TelematicsFines#��Z 6,   $��12��6f   $��1��6t   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5G   
   -   5         ��Name$ stackPnlTelmFine��5G        �� Orientation$  
Horizontal=�6/   $��12��6H   $��3��6V   $	��350��6f   $��40��6r   $
��Left=�6~   .+�5H      �� -   5        $ txtTelematicFinesName��5H      $	��175��65   $��40��6A   $��20(�6M   $��Segoe UI)�6[   $��Medium"�6q   $��Stretch=�6�   $��Bottom=�6�   	 . 6�   �� 5I      -   5        $ txtTelematicFinesAmmount��5I      $	��175��68   $��40��6D   $��20(�6Q   $��Segoe UI)�6_   $��Medium"�6u   $��Right=�6�   $��Bottom=�6�   	 . 6�   5J      �� 5L   
   -   5        $ lblImpounding��5L      !   
Impounding#��Z! 6'   $��14��6\   $��1��6j   $��Segoe UI)�6z   $��Medium"�6�   $��25(�6�   �� 5M   
   -   5        $ txtImpounding��5M      $��14��6)   $��0��67   $��3��6@   $	��350��6P   $��40��6\   $��20(�6h   $��Segoe UI)�6v   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5Q   
   -   5        $ lblGracePeriod��5Q      "   ImpoundGracePeriod#��Z" 6(   $��16��6e   $��1��6s   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5R   
   -   5        $ txtGracePeriod��5R      $��16��6*   $��3��68   $	��350��6H   $��40��6T   $��20(�6`   $��Segoe UI)�6n   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5W   
   -   5        $ lblPrintLang��5W      #   SelectLanguage2#��Z# 6&   $��Segoe UI)�6`   $��Medium"�6v   $��25(�6�   $��18��6�   $��1��6�   �� 5Y   
   -   5        $ ��Name$$ cmboxLanguageSelction��5Y      $��18��62   $	��350��6@   $��3��6L   % �� 
IsEditable% . & �� IsTextSearchEnabled& .  !  cmbLargeSizeForComplexScreens#��[  6�   $
��Left=�6�   $��Stretch=�6�   (� 5_   
   $y�	Collapsed=�6   $��28��6)   $��10��65   $��3��6C   $
��Left=�6T   $��Center=�6p   .��5a      	� -   5        ' 	�Name$' rdoBtnEngLang��5a      (   English#��Z( 62   $
��Left=�6�   $��Center=�6�   $��Segoe UI)�6�   $��Medium"�6�   5b      (� 5c   
   $y�	Collapsed=�6   $��28��6)   $��10��65   $��3��6C   $��Right=�6T   $��Center=�6q   .��5d      	� -   5        $' rdoBtnArabLang��5d      )   Arabic#��Z) 63   $��Right=�6�   $��Center=�6�   $��Segoe UI)�6�   $��Medium"�6�   5e      �� 5g   
   -   5        $ btnStackePanel��5g      $��20��6-   $��3��6;   $  
Horizontal=�6K   $��Center=�6d   $��Right=�5h      .+�5i      �  5k      �� 5l      $��* �6$    5o      �� 5p      $��* �6'   �� 5q      $��* �6'   .+�5s      �� -   5        * ��Name$* btnPrint��5s   �   +   BtnPrint#��Z+ 6   $��0��6L   $��1��6Y   $y�Visible=�6i     ButtontoProceedHalfControl#��[ 6~   $
��Left=�6�   $��20,0,0,0q�6�   6�   �� 5t      -   5        $* btnback��5t   �   ,   BtnBack#��Z, 6   $��0��6K   $��0��6X   $y�Visible=�6h     ButtonBackHalfControl#��[ 6}   $
��Left=�6�   $��0,0,0,0q�6�   6�   �� 5u      -   5        - ��Name$- imagebtnBack��5u      $y�	Collapsed=�6.   $&�� ..\Images\Buttons\Small\Back.png��6E   $	��150��6o   $��9��6|   $��3��6�   $��Right=�6�   $��Stretch=�6�   6  �� 5v      -   5        $- imageBtnPrint��5v      $y�	Collapsed=�6/   $'��!..\Images\Buttons\Small\Print.png��6F   $	��150��6q   $��9��6~   $��40,0,0,0q�6�   $��3��6�   $��Right=�6�   $��Stretch=�6�   6!  5w      5{      5|      5      