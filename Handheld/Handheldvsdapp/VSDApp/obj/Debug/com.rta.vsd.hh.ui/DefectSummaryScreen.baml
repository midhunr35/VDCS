   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5   	   2    ,VSDApp.com.rta.vsd.hh.ui.DefectSummaryScreen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   6
propertiesclr-namespace:VSDApp.Properties     5   	   -   5        !   VSDApp.Properties.Resources    DefectSummary � Title# Z  5   	   $	��300��5   
   $	��550��6    � 
ResizeMode$ NoResize=�6#    � WindowStyle$ SingleBorderWindow=�6R    � WindowStartupLocation$ CenterOwner=�6t   
�������
���  �� � 	Resources 5       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5      � -   5         �Name$ MainGrid��5      	 � RowDefinitions	 5   
   �� 5      $��0 �6   �� 5      $��* �6   �� 5      $��0 �6   �� 5      $
��Auto �6   �� 5      $��0 �6   
 � ColumnDefinitions
 5   
   �� 5      $��5 �6   �� 5      $��* �6   �� 5      $��5 �6   .+�5"   
   &   System.Windows.Controls.DataGrid  -   5          Name$ grdAddedDefects��5"      $
��Auto��6,   $
��Auto��6:   $��1��6G   $��1��6T      AutoGenerateColumns .  $��Stretch=�5#      $��Segoe UI)�6=   $��Medium"�6S   $��Stretch=�6g   $��15(�6�      CanUserDeleteRows .    CanUserAddRows .  $��Stretch=�6�    �� SelectedItem 5"   e   �� *SelectedItem+ �� Mode$ TwoWay=���6�   �� *AddedDefects+ �� UpdateSourceTrigger$ PropertyChanged=� 5$      =  7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType �� Property �� Value��%�   '        �� 5%      "  6   .�5&      ��  � �� �#FF333463   �� 5'       � ��
 �����64   �� 5(       � ��$ 25��60   5)      5*         Columns 5+      0  *System.Windows.Controls.DataGridTextColumn  5,         Type,  &System.Windows.Controls.DataGridColumn   Header# Z 6&      
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6g      FontSize$ 15Q�6q      
FontFamily$ Segoe UI)�6�      
FontWeight$ Medium"�6�      
Visibility$ 	Collapsed=�6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6�   �� *
DefectType+6�     5-         SubCategory2# Z 6%    . $ * 6n   $ 15Q�6x   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *DefectSubCat+6�     5.          Name# Z  6%    . $ * 6e   $ 15Q�6o   $ Segoe UI)�6�   $ Medium"�6�    6}   �� *
DefectName+6�     5/      !   ActualValue# Z! 6%    . $ * 6m   $ 15Q�6w   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *DefectValue+6�     50      -   5        lblgrdSeverityEn��"   Severity# Z" 50   ?    . $	 100 6�   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *DefectSeverity+6�     51      -   5        lblgrdSeverityAr��# Z" 51   ?    . $	 100 6�   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *DefectSeverityAr+6�   55      � 57   
   -   5        $ internalGrid��57      $��3��6%   $��1��62   	 59      �� 5:      $��* �6    
 5=      �� 5>      $��* �6#   �� 5?      $��* �6#   .+�5A      �� -   5        # ��Name$# stckPanelbtn��5A      $��0��6/   $��1��6<   $ �� Orientation$$ 
Horizontal=�6M   $��Center=�6f   $��Right=�5B      $��0 0 5 0q�62   .+�5C      �� -   5        	%   No#��Z% 5C         ButtonBackHalfControl#��[  6F   $��15(�6u   $��0 5 10 5q�6�   $��24��6�   $��80��6�   �� 5F      -	   5        
&   Yes#��Z& 5F        ButtontoProceedHalfControl#��[ 6^   $��15(�6�   $��15 5 5 5q�6�   $��24��6�   $��80��6�   5H      �� 5I      -
   5        ' ��Name$' lblConfirmation��5I      $��0��61   $��0��6>   $��Center=�6N   $��5 5 5 5q�6l   (   lblSubmitViolation#��Z( 6}   5J      5K      5L      