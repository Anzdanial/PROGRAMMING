import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

class StringUtils {
	public String concatenate(String str1, String str2) {
		if (str1 == null || str2 == null) {
			throw new IllegalArgumentException("Input strings cannot be null");
		}
		return str1 + str2;
	}

	public int findLength(String str) {
		if (str == null) {
			throw new IllegalArgumentException("Input string cannot be null");
		}
		return str.length();
	}

	public String toUpperCase(String str) {
		if (str == null) {
			throw new IllegalArgumentException("Input string cannot be null");
		}
		return str.toUpperCase();
	}
}

class StringUtilsTesting{
	private StringUtils test;
	StringUtilsTesting(){
		test = new StringUtils();
	}
	@Test
	public void concatenateTest(){
		String str = "Hello";
		String st2 = "World";
		assertEquals("HelloWorld",test.concatenate(str,st2), "Successful Concatenation");
	}

	@Test
	public void findLengthTest(){
		String str = "Anas";
		assertEquals(4,str.length());
	}
}


public class Main {
	public static void main(String[] args) {
		StringUtilsTesting test = new StringUtilsTesting();
		test.concatenateTest();
	}
}