   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5   	   
 VSDApp+   %VSDApp.com.rta.vsd.hh.ui.CameraWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   -   5          � Title$  Camera Window��5   	   $
��auto��6   $
��auto��6-   $��Center=�5   	   $	��Top=�6&   .��5      � .+�5	   
   ��  �� Orientation$ Vertical=�6   .+�5
      �� -   5         ��Name$ stkPanelR0C0��5
      $ 
Horizontal=�6/   $y�	Collapsed=�6H   .+�5      �� -   5         ��Name$ 
chkBxFlash��5      $��Center=�60    d� 	IsChecked$
 TrueY�6K   $��10,0,0,0q�6u   .��5      �� -   5         ��Name$ imgCamFlash��5      5      �� 5      -   5         ��Name$ cmbBxActiveCamera��5      $	��200��67   $y�Hidden=�6y   5      7  1System.Windows.Forms.Integration.WindowsFormsHost  5      -   5          Name$ winFrmHostPnlCmraView��5         Child. 5          System.Windows.Forms.Panel  -   5        pnlCameraView��"   System.Windows.Forms.Control	   	BackColorU QSystem.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a#  System.Drawing.ColorConverter$	 Black 5   2   5      �� 5      -   5        $ stkPanelR2C0��5      $ 
Horizontal=�6/   $��10,10,10,10q�6H   .+�5      �� -	   5        
 ��Name$
 btnCaptureImg��5      $��Segoe UI)�60   $��Medium"�6F   
���  ��
�������$��Take Picture��6~   $��10,0,3,0q�6�   $��80��6�   �� 5      -
   5        $
 btnExit��5      $��Segoe UI)�6*   $��Medium"�6@   
�������
�������$
��Exit��6z   $��80��6�   $��3,0,10,0q�6�   �� 5      -   5         ��Name$ lblDefectDetails��5      $�� ��62   $
��auto��6>   $��	10,0,10,0q�6K   $
��Bold"�6^   5      5      5      5      