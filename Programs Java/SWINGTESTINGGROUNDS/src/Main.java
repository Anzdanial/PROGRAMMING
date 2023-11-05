import javax.swing.*;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;


class ComponentListenerResource implements ListSelectionListener, ActionListener {
	private DefaultListModel<String> listModelResource;
	private DefaultListModel<String> listModelSkill;
	private JList<String> listResource;
	private JList<String> listSkill;
	private JTextField fieldResource;
	private JTextField fieldSkill;
	private boolean selectedResource;

	ComponentListenerResource(JList<String> listResource, JList<String> listSkill, DefaultListModel<String> listModelResource, DefaultListModel<String> listModelSkill, JTextField fieldResource, JTextField fieldSkill) {
		this.listResource = listResource;
		this.listSkill = listSkill;
		this.listModelResource = listModelResource;
		this.listModelSkill = listModelSkill;
		this.fieldResource = fieldResource;
		this.fieldSkill = fieldSkill;
		selectedResource = false;
	}

	public void actionPerformed(ActionEvent e) {
		listModelResource.addElement(fieldResource.getText());
		if(selectedResource){
			listModelSkill.addElement(fieldSkill.getText());
		}
	}

	public void valueChanged(ListSelectionEvent e) {
		if (!e.getValueIsAdjusting()) {
			int index = e.getLastIndex();
			String selectedValue = listResource.getSelectedValue();
		}
	}
}




public class Main{
	static public void main(String [] args){

	}
}