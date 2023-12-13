package View;

import javax.swing.*;
import java.awt.*;

public class StrategyView {

    // Fields and Buttons
    private ButtonGroup buttonGroup;
    private JRadioButton exactMatchButton;
    private JRadioButton skillOnlyMatchButton;
    private JButton generateButton;
    private JList<Object> matchedTalentsList;

    // Getters and Setters
    public ButtonGroup getButtonGroup() {
        return buttonGroup;
    }

    public void setButtonGroup(ButtonGroup buttonGroup) {
        this.buttonGroup = buttonGroup;
    }

    public JRadioButton getExactMatchButton() {
        return exactMatchButton;
    }

    public void setExactMatchButton(JRadioButton exactMatchButton) {
        this.exactMatchButton = exactMatchButton;
    }

    public JRadioButton getSkillOnlyMatchButton() {
        return skillOnlyMatchButton;
    }

    public void setSkillOnlyMatchButton(JRadioButton skillOnlyMatchButton) {
        this.skillOnlyMatchButton = skillOnlyMatchButton;
    }

    public JButton getGenerateButton() {
        return generateButton;
    }

    public void setGenerateButton(JButton generateButton) {
        this.generateButton = generateButton;
    }

    public JList<Object> getMatchedTalentsList() {
        return matchedTalentsList;
    }

    public void setMatchedTalentsList(JList<Object> matchedTalentsList) {
        this.matchedTalentsList = matchedTalentsList;
    }

    // Constructor
    public StrategyView() {
        exactMatchButton = new JRadioButton("Exact Match");
        skillOnlyMatchButton = new JRadioButton("Skill-Only Match");
        buttonGroup = new ButtonGroup();
        matchedTalentsList = new JList<>();
        buttonGroup.add(exactMatchButton);
        buttonGroup.add(skillOnlyMatchButton);
        generateButton = new JButton("Generate");
        matchedTalentsList.setPreferredSize(new Dimension(400, 150));

    }

    public JPanel getMainPanel() {
        JPanel panel = new JPanel();
        panel.add(exactMatchButton);
        panel.add(skillOnlyMatchButton);
        panel.add(generateButton);
        panel.add(new JScrollPane(matchedTalentsList));
        return panel;
    }
}