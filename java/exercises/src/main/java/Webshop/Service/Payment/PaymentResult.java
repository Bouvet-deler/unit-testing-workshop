package Webshop.Service.Payment;

public class PaymentResult {
    private final PaymentResultCode code;
    private final String errorMessage;

    public PaymentResult(PaymentResultCode code) {
        this(code, null);
    }

    public PaymentResult(PaymentResultCode code, String errorMessage) {
        this.code = code;
        this.errorMessage = errorMessage;
    }

    public boolean isSuccess() {
        return code == PaymentResultCode.SUCCESS;
    }

    public PaymentResultCode getCode() {
        return code;
    }

    public String getErrorMessage() {
        return errorMessage;
    }
}

