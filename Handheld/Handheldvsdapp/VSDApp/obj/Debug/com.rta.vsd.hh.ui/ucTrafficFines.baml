   M S B A M L   `   `   `  ����A  =VSDApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:VSDApp.PropertiesVSDApp.Properties  5      -    'VSDApp.com.rta.vsd.hh.ui.ucTrafficFines   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6
propertiesclr-namespace:VSDApp.Properties     5      -   5          � 	Resources  5	       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $  ..\ResourceDictionary.xaml?�6%   %    5      .��5   
   �  � RowDefinitions 5   
   �� 5      $��10 �6   �� 5      $��* �6   �� 5      $��5 �6   �� 5      $
��Auto �6   �� 5      $��5 �6    � ColumnDefinitions 5   
   �� 5      $��5 �6   �� 5      $��* �6   �� 5       $��5 �6   .+�5#   
   �� $
��Auto=�6   $
��Auto=�6:   $��1��6_   $��1��6m   .��5$      �� 
�������.+�5&      &   System.Windows.Controls.DataGrid  -   5          Name$ grdTrafficFines��5&         AutoGenerateColumns .  $   HorizontalScrollBarVisibility$ Visible=�6O   $
��Left=�6w   ��. $��Visible=�5'   �   $
��Auto=�5(         CanUserAddRows .  $��Segoe UI)�6f   $��Medium"�6|   $��15(�6�   ��5'      �� *violationData+	 �� UpdateSourceTrigger$	 PropertyChanged=�
   Columns
 5+      0  *System.Windows.Controls.DataGridTextColumn  5,      -   5        headDescriptionEN��   FontSize$ 15Q�5,   D   ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6d      
FontFamily$ Segoe UI)�6r      
FontWeight$ Medium"�6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6�   �� *DescriptionEn+   Header 5-      �� 5.       �� Orientation$ Vertical=�6)   $
��Auto��6@   .+�5/   "   �� -   5         ��Name$ txtDespFilter��5/   *   $	��280��6A    �� 
IsReadOnly .  6�   �� 50   "   $��Description��6-   51      55        58      -   5        headFineAmmountEn��$ Fine Amount��58   D    . $
 Auto 6l   $ 15Q�6y   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *FineAmmount+6�     59      -   5        headBlackPointEn��$ Black Points��59   C    . $
 Auto 6l   $ 15Q�6y   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *BlackPoints+6�     5:      -   5        headVehicelConfisPeriodEn��$! Vehicle Confiscation Period��5:   L    . $
 Auto 6�   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *VehicleConfiscationPeriod+6�     5<      -   5        
headTypeEn�� . $ 15Q�5<   O   $ Segoe UI)�6]   $ Medium"�6s    6�   �� *Type+ 5=      �� 5>      $ Vertical=�6)   $
��Auto��6@   .+�5?   "   �� -	   5        $ 	txtTypeEn��5?   *   $��90��6=    .  6l   �� 5@   "   $
��Type��6-   5A      5E        5F      -
   5        
headTypeAr�� . $ 15Q�5F   O   $ Segoe UI)�6]   $ Medium"�6s    6�   �� *TypeAr+ 5G      �� 5H      $ Vertical=�6)   $
��Auto��6@   .+�5I   "   �� -   5        $ 	txtTypeAr��5I   *   $��90��6=    .  6n   �� 5J   "   $��
النوع��6-   5K      5O        5Q      -   5        headVehicelConfisPeriodAr��$" مدة حجز المركبة��5Q   L    . $
 Auto 6x   $ 15Q�6�   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *VehicleConfiscationPeriodAr+6�     5R      -   5        headBlackPointsAr��$! النقاط السوداء��5R   D    . $
 Auto 6o   $ 15Q�6|   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *BlackPointsAr+6�     5S      -   5        headFineAmmountAr��$ قيمة الغرامةة��5S   D    . $
 Auto 6n   $ 15Q�6{   $ Segoe UI)�6�   $ Medium"�6�    6�   �� *FineAmmountsAr+6�     5U      -   5        headDesctiptionAr��$ 15Q�5U   D    . $ Segoe UI)�6d   $ Medium"�6z    6�   �� *DescriptionAr+ 5V      �� 5W      $ Vertical=�6)   $	��150��6@   .+�5X   "   �� -   5        $ txtDespFilterAr��5X   *   $
��Auto��6C    .  6|   �� 5Y   "   $��وصف المخالفة��6-   5Z      5^      5a      5d      5e      �� 5f   
   $��3��6   $��1��6#   
�������.+�5g      �� -   5         ��Name$ imagebtnback��5g      $y�	Collapsed=�6*   $&�� ..\Images\Buttons\Small\Back.png��6A   $��Stretch=�6n   6�   �� 5i   
   -   5         ��Name$ btnBack��5i      !   VSDApp.Properties.Resources   BtnBack#��Z 6#      ButtonBackHalfControl#��[  6U   $��3��6�   $��1��6�   $��Right=�6�   $��0,0,0,0q�6�   6�   5j      5k      5p      