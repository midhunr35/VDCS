   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      1    +VSDApp.com.rta.vsd.hh.ui.ucViolationSummary   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      -   5        $
��Auto��5      $
��Auto��6   $��Stretch=�6)   $��Stretch=�6G   $��Stretch=�5      $��Stretch=�6*   
�������  � 	Resources  5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      �� $
��Auto=�6   $
��Auto=�66   .��5   
   �  � RowDefinitions 5   
   �� 5      $��1 �6    �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��5 �6    �� 5      $
��Auto �6    �� 5      $��5 �6    �� 5      $
��Auto �6    �� 5      $��5 �6    �� 5      $
��Auto �6    �� 5       $��5 �6    �� 5!      $
��Auto �6    �� 5"      $��5 �6    �� 5#      $
��Auto �6    �� 5$      $��5 �6    �� 5%      $
��Auto �6    �� 5&      $��5 �6   �� 5'      $
��Auto �6   �� 5(      $��5 �6   �� 5)      $
��Auto �6   �� 5*      $��5 �6   �� 5+      $
��Auto �6    �� 5,      $��* �6   �� 5-      $��* �6    � ColumnDefinitions 5/      �� 50      $��* �6#   �� 51      $
��Auto �6#   �� 52      $��10 �6#   �� 53      $
��Auto �6#   �� 54      $��* �6#   .+�56   
   �� -   5         ��Name$ 
lblViolSum��56      !   VSDApp.Properties.Resources   ViolationSummary#��Z 6$   $��Segoe UI)�6_   $��Medium"�6u   $��30(�6�   $��2��6�   $��3��6�   $��1��6�   $
��Left=�6�   $��Stretch=�6�   �� 57   
   -   5        $ lblOvrAllSev��57         OverAllViolationSeverity#��Z 6&   $��4��6i   $��1��6v   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 58      -   5         ��Name$ txtboxViolationSeverity��58      $��4��67   $��3��6D   $	��350��6T   $��40��6`   $��20(�6l   $��Segoe UI)�6z   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 �� 
IsReadOnly	 . 6�   �� 59   
   -   5        $ 	lblVioSum��59      
   ViolationID#��Z
 6#   $��6��6Y   $��1��6f   $��Segoe UI)�6v   $��Medium"�6�   $��25(�6�   �� 5:      -   5        $ txtViolationID��5:      $��6��6.   $��3��6;   $	��350��6K   $��40��6W   $��20(�6c   $��Segoe UI)�6q   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5;   
   -   5        $ 
lblDueDate��5;         DueDate#��Z 6$   $��8��6V   $��1��6c   $��Segoe UI)�6s   $��Medium"�6�   $��25(�6�   �� 5<      -   5        $ txtboxDueDate��5<      $��8��6-   $��3��6:   $	��350��6J   $��40��6V   $��20(�6b   $��Segoe UI)�6p   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5>   
   -	   5        $ lblTechnicalFines��5>         TechnicalFines#��Z 6+   $��10��6d   $��1��6r   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   &   System.Windows.Controls.DataGrid  5?   
   -
   5          Name$ grdAddedFines��5?      $	��350��6*   $��70��66   $��10��6B   $��3��6P      AutoGenerateColumns .  $
��Left=�5@      $��Segoe UI)�6:   $��Medium"�6P   $��Stretch=�6d   $��15(�6�      CanUserDeleteRows .    CanUserAddRows .  $��Stretch=�6�    �� SelectedItem 5?   a   �� *SelectedItem+ �� Mode$ TwoWay=���6�   �� *
AddedFines+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5B      0  *System.Windows.Controls.DataGridTextColumn  5C      -   5        lblFineName��   Fine,  &System.Windows.Controls.DataGridColumn   Header# Z 5C   :      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6{      FontSize$ 15Q�6�      
FontFamily$ Segoe UI)�6�      
FontWeight$ Medium"�6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6�   �� *
FineName+6�     5D      -   5        lblFineAmmount��   Ammount# Z 5D   =    . $ * 6�   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *FineAmmount+6�   5G      �� 5H   
   -   5        $ lblTelematicsFines��5H         TelematicsFines#��Z 6,   $��12��6f   $��1��6t   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5I      -   5         ��Name$ stackPnlTelmFine��5I        �� Orientation$  
Horizontal=�63   $��12��6L   $��3��6Z   $	��350��6j   $��40��6v   $
��Left=�6�   .+�5J      �� -   5        $ txtTelematicFinesName��5J      $	��175��65   $��40��6A   $��20(�6M   $��Segoe UI)�6[   $��Medium"�6q   $��Stretch=�6�   $��Bottom=�6�   	 . 6�   �� 5K      -   5        $ txtTelematicFinesAmmount��5K      $	��175��68   $��40��6D   $��20(�6Q   $��Segoe UI)�6_   $��Medium"�6u   $��Right=�6�   $��Bottom=�6�   	 . 6�   5L      �� 5N   
   -   5        $ lblImpounding��5N      !   
Impounding#��Z! 6'   $��14��6\   $��1��6j   $��Segoe UI)�6z   $��Medium"�6�   $��25(�6�   �� 5O      -   5        $ txtImpounding��5O      $��14��6-   $��3��6;   $	��350��6K   $��40��6W   $��20(�6c   $��Segoe UI)�6q   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5S   
   -   5        $ lblGracePeriod��5S      "   ImpoundGracePeriod#��Z" 6(   $��16��6e   $��1��6s   $��Segoe UI)�6�   $��Medium"�6�   $��25(�6�   �� 5T      -   5        $ txtGracePeriod��5T      $��16��6.   $��3��6<   $	��350��6L   $��40��6X   $��20(�6d   $��Segoe UI)�6r   $��Medium"�6�   $
��Left=�6�   $��Bottom=�6�   	 . 6�   �� 5Y   
   -   5        $ lblPrintLang��5Y      #   SelectLanguage2#��Z# 6&   $��Segoe UI)�6`   $��Medium"�6v   $��25(�6�   $��18��6�   $��1��6�   �� 5[      -   5        $ ��Name$$ cmboxLanguageSelction��5[      $��18��66   $	��350��6D   $��3��6P   % �� 
IsEditable% . & �� IsTextSearchEnabled& .  !  cmbLargeSizeForComplexScreens#��[  6�   $
��Left=�6�   $��Stretch=�6�   �� 5`   
   -   5        $ btnStackePanel��5`      $��20��6-   $��3��6;   $  
Horizontal=�6K   $��Center=�6d   $��Right=�5a      .+�5b      �  5d      �� 5e      $��* �6$    5h      �� 5i      $��* �6'   �� 5j      $��* �6'   .+�5l      �� -   5        ' ��Name$' btnPrint��5l   �   (   BtnPrint#��Z( 6   $��0��6L   $��1��6Y   $y�Visible=�6i     ButtontoProceedHalfControl#��[ 6~   $
��Left=�6�   $��20,0,0,0q�6�   6�   �� 5m      -   5        $' btnback��5m   �   )   BtnBack#��Z) 6   $��0��6K   $��0��6X   $y�Visible=�6h     ButtonBackHalfControl#��[ 6}   $
��Left=�6�   $��0,0,0,0q�6�   6�   �� 5n      -   5        * ��Name$* imagebtnBack��5n      $y�	Collapsed=�6.   $&�� ..\Images\Buttons\Small\Back.png��6E   $	��150��6o   $��9��6|   $��3��6�   $��Right=�6�   $��Stretch=�6�   6  �� 5o      -   5        $* imageBtnPrint��5o      $y�	Collapsed=�6/   $'��!..\Images\Buttons\Small\Print.png��6F   $	��150��6q   $��9��6~   $��40,0,0,0q�6�   $��3��6�   $��Right=�6�   $��Stretch=�6�   6!  5p      5t      5u      5w      5x      