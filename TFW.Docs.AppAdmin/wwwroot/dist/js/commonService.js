function CommonService({
  defaultConfirmTitle,
  defaultConfirmBtnText,
  defaultSuccessTitle,
  defaultErrorTitle,
  defaultOkBtnText,
  defaultErrorHtml,
  defaultToastOptions = {
    toast: true,
    position: 'bottom-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
      toast.addEventListener('mouseenter', Swal.stopTimer)
      toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
  }
}) {
  const getApiErrorMessage = (data) => {
    let html = data?.responseJSON?.message;
    if (html) {
      html = html.replace('\n', '<br/>');
    } else {
      html = defaultErrorHtml;
    }
    return html;
  };

  const toast = Swal.mixin(defaultToastOptions);

  const showError = ({
    heightAuto = false,
    title = defaultErrorTitle,
    text = null,
    html = null,
    icon = 'error',
    confirmButtonText = defaultOkBtnText,
    useToast = true,
  }) => {
    const service = useToast ? toast : Swal;
    const options = { icon, title, text, html, confirmButtonText };
    if (!useToast) {
      options.heightAuto = heightAuto;
    }
    return service.fire(options);
  };

  return {
    toast,

    isLocal(url) {
      return location.origin === new URL(url, location.origin).origin;
    },

    changeLoading: (show) => {
      if (show) {
        $('html').addClass('overflow-hidden');
        $('body').addClass('overflow-hidden');
        $('.modal-loading').show();
      }
      else {
        $('html').removeClass('overflow-hidden');
        $('body').removeClass('overflow-hidden');
        $('.modal-loading').hide();
      }
    },

    showSuccess: ({
      heightAuto = false,
      title = defaultSuccessTitle,
      text = null,
      html = null,
      icon = 'success',
      confirmButtonText = defaultOkBtnText,
      useToast = false,
    }) => {
      const service = useToast ? toast : Swal;
      const options = { icon, title, text, html, confirmButtonText };
      if (!useToast) {
        options.heightAuto = heightAuto;
      }
      return service.fire(options);
    },

    showError,

    getApiErrorMessage,

    handleApiError: (data) => {
      const html = getApiErrorMessage(data);
      showError({
        html
      });
    },

    showConfirm: ({
      heightAuto = false,
      title = defaultConfirmTitle,
      text = null,
      html = null,
      icon = 'warning',
      showCancelButton = true,
      confirmButtonColor = '#3085d6',
      cancelButtonColor = '#d33',
      confirmButtonText = defaultConfirmBtnText
    }) => {
      return Swal.fire({
        heightAuto, title, text, html, icon, showCancelButton, confirmButtonColor, cancelButtonColor, confirmButtonText
      });
    }
  };
}
