interface Props {
  year: number;
  onPrevYear: () => void;
  onNextYear: () => void;
  semester: number;
  onChangeSemester: (semester: number) => void;
  onLoadExample: () => void; // added prop
}

const SemesterHeaderToolbar: React.FC<Props> = ({
  year,
  onPrevYear,
  onNextYear,
  semester,
  onChangeSemester,
  onLoadExample,
}) => {
  return (
    <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200 dark:border-gray-700 rounded-t-2xl">
      {/* Left Section */}
      <div className="flex items-center gap-2">
        <button
          onClick={onPrevYear}
          className="rounded-lg border border-gray-300 bg-white px-2 py-1 text-sm hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-800 dark:text-white"
        >
          ←
        </button>
        <button
          onClick={onNextYear}
          className="rounded-lg border border-gray-300 bg-white px-2 py-1 text-sm hover:bg-gray-50 dark:border-gray-600 dark:bg-gray-800 dark:text-white"
        >
          →
        </button>
        <button
          onClick={onLoadExample}
          className="rounded-lg border border-blue-300 bg-blue-100 px-2 py-1 text-sm text-blue-800 hover:bg-blue-200 dark:border-blue-600 dark:bg-blue-900 dark:text-white"
        >
          Load Example 1
        </button>
      </div>

      {/* Center Section */}
      <div className="text-lg font-semibold text-gray-800 dark:text-white">
        Start Semester: {year}S{semester}
      </div>

      {/* Right Section */}
      <div className="flex items-center bg-gray-100 dark:bg-gray-700 rounded-lg overflow-hidden">
        {[1, 2].map((s) => (
          <button
            key={s}
            onClick={() => onChangeSemester(s)}
            className={`px-3 py-1 text-sm font-medium ${
              semester === s
                ? "bg-white dark:bg-gray-900 text-brand-600"
                : "text-gray-500 hover:bg-white/60 dark:hover:bg-gray-800"
            }`}
          >
            Semester {s}
          </button>
        ))}
      </div>
    </div>
  );
};

export default SemesterHeaderToolbar;